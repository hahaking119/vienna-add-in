using System;
using System.Collections.Generic;
using System.IO;
using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Importer;
using File=System.IO.File;

namespace CCLImporter
{
    internal static class Program
    {
        private const bool Debug = false;

        private const int MaximumTaggedValueLength = 255;
        public static int LineNumber;

        public static void Main(string[] args)
        {
//            ImportCCL("CCL08B");
            ImportCCL("CCL09A");
        }

        private static void ImportCCL(string cclVersion)
        {
            var eaRepository = new Repository();
            string originalRepoPath = Directory.GetCurrentDirectory() + string.Format(@"\..\..\resources\{0}\Repository-with-CDTs.eap", cclVersion);
            string targetRepoPath = originalRepoPath.WithoutSuffix(".eap") + "-and-CCs.eap";
            File.Copy(originalRepoPath, targetRepoPath, true);
            eaRepository.OpenFile(targetRepoPath);
            var ccRepository = new CCRepository(eaRepository);
            var bLibrary = ccRepository.LibraryByName<IBLibrary>("bLibrary");
            var cdtLibrary = (ICDTLibrary) bLibrary.FindChildByName("CDTLibrary");

            var cdts = new Dictionary<string, ICDT>();
            foreach (ICDT cdt in cdtLibrary.Elements)
            {
                cdts[cdt.Name] = cdt;
            }

            ICCLibrary ccLibrary = bLibrary.CreateCCLibrary(new LibrarySpec
                                                            {
                                                                Name = "CCLibrary",
                                                                VersionIdentifier = cclVersion
                                                            });

            accResolver = new ACCResolver(ccLibrary);

            StreamReader reader = File.OpenText(string.Format(@"..\..\resources\{0}\{0}-CCs.txt", cclVersion));
            String line;
            var accSpecs = new List<ACCSpec>();
            ACCSpec accSpec = null;
            while ((line = reader.ReadLine()) != null)
            {
                LineNumber++;
                if (LineNumber < 2) continue;

                if (Debug)
                {
                    if (LineNumber > 150) break;
                }

                Record record = GetRecord(line);

                switch (record.ElementType)
                {
                    case "ACC":
                        CheckACCRecord(record);
                        accSpec = new ACCSpec
                                  {
                                      UniqueIdentifier = record.UniqueUNAssignedID,
                                      Name = GetACCName(record),
                                      Definition = record.Definition.LimitTo(MaximumTaggedValueLength),
                                      BusinessTerms = ToArray(record.BusinessTerms.LimitTo(MaximumTaggedValueLength)),
                                      UsageRules = ToArray(record.UsageRules.LimitTo(MaximumTaggedValueLength)),
                                      VersionIdentifier = record.Version
                                  };
                        accSpecs.Add(accSpec);
                        break;
                    case "BCC":
                        if (accSpec == null)
                        {
                            throw new Exception("The first record must specify an ACC.");
                        }
                        CheckBCCRecord(record, accSpec.Name);
                        var bccSpec = new BCCSpec
                                      {
                                          UniqueIdentifier = record.UniqueUNAssignedID,
                                          Definition = record.Definition.LimitTo(MaximumTaggedValueLength),
                                          DictionaryEntryName = record.DictionaryEntryName,
                                          Name = QualifiedName(record.PropertyTermQualifiers, record.PropertyTerm),
                                          BusinessTerms = ToArray(record.BusinessTerms.LimitTo(MaximumTaggedValueLength)),
                                          UsageRules = ToArray(record.UsageRules.LimitTo(MaximumTaggedValueLength)),
                                          SequencingKey = record.SequenceNumber,
                                          LowerBound = MapOccurrence(record.OccurrenceMin),
                                          UpperBound = MapOccurrence(record.OccurrenceMax),
                                          VersionIdentifier = record.Version,
                                          Type = FindCDT(record.RepresentationTerm.AsName(), cdts)
                                      };
                        if (bccSpec.Type == null)
                        {
                            Console.WriteLine("WARNING: Skipping line {0}: CDT not found: <{1}>.", LineNumber, record.RepresentationTerm);
                            continue;
                        }
                        accSpec.AddBCC(bccSpec);
                        break;
                    case "ASCC":
                        if (accSpec == null)
                        {
                            throw new Exception("The first record must specify an ACC.");
                        }
                        CheckASCCRecord(record, accSpec.Name);
                        var asccSpec = new ASCCSpec
                                       {
                                           UniqueIdentifier = record.UniqueUNAssignedID,
                                           Definition = record.Definition.LimitTo(MaximumTaggedValueLength),
                                           Name = QualifiedName(record.PropertyTermQualifiers, record.PropertyTerm),
                                           BusinessTerms = ToArray(record.BusinessTerms.LimitTo(MaximumTaggedValueLength)),
                                           UsageRules = ToArray(record.UsageRules.LimitTo(MaximumTaggedValueLength)),
                                           SequencingKey = record.SequenceNumber,
                                           LowerBound = MapOccurrence(record.OccurrenceMin),
                                           UpperBound = MapOccurrence(record.OccurrenceMax),
                                           VersionIdentifier = record.Version,
                                           ResolveAssociatedACC = accResolver.ResolveACC(record.AssociatedObjectClass.AsName()),
                                       };
                        accSpec.AddASCC(asccSpec);
                        break;
                    default:
                        Console.WriteLine("WARNING: Skipping line {0}.", LineNumber);
                        break;
                }
            }
            reader.Close();

            try
            {
                ACCImporter.ImportACCs(ccLibrary, accSpecs);
            }
            finally
            {
                eaRepository.CloseFile();
            }
            Console.WriteLine("INFO: Number of ACCs: " + accSpecs.Count);
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        private static ACCResolver accResolver;

        private static string MapOccurrence(string occurrence)
        {
            if (string.IsNullOrEmpty(occurrence))
            {
                return "1";
            }
            return occurrence == "unbounded" ? "*" : occurrence;
        }

        private static void CheckASCCRecord(Record record, string accName)
        {
            //            record.UniqueUNAssignedID;
            record.DictionaryEntryName.CheckedField("Dictionary Entry Name").Must().Be(record.ObjectClassTerm + ". " + record.PropertyTerm + ". " + record.AssociatedObjectClass);
            record.Definition.CheckedField("Definition").Must().Not().BeEmpty().And().Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.ObjectClassTerm.AsName().CheckedField("Object Class Term").Must().Be(accName);
            record.PropertyTermQualifiers.CheckedField("Property Term Qualifier(s)").Should().BeEmpty();
            record.PropertyTerm.CheckedField("Property Term").Must().Not().BeEmpty();
            record.DatatypeQualifiers.CheckedField("Data Type Qualifier(s)").Should().BeEmpty();
            record.RepresentationTerm.CheckedField("Representation Term").Should().BeEmpty();
            record.QualifiedDataTypeUID.CheckedField("Qualified Data Type UID").Should().BeEmpty();
            record.AssociatedObjectClassTermQualifiers.CheckedField("Associated Object Class Term Qualifier(s)").Should().BeEmpty();
            record.AssociatedObjectClass.CheckedField("Associated Object Class").Must().Not().BeEmpty();
            record.BusinessTerms.CheckedField("Business Terms").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.UsageRules.CheckedField("Usage Rules").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            //            record.SequenceNumber.CheckedField("Sequence Number");
            //            record.OccurrenceMin.CheckedField("Occurrence Min");
            //            record.OccurrenceMax.CheckedField("Occurrence Max");
            //            record.Version;
        }

        private static string QualifiedName(string qualifiers, string name)
        {
            if (qualifiers.Length > 0)
            {
                return qualifiers + "_" + name;
            }
            return name.AsName();
        }

        private static void CheckBCCRecord(Record record, string accName)
        {
//            record.UniqueUNAssignedID;
            record.DictionaryEntryName.CheckedField("Dictionary Entry Name").Must().Be(record.ObjectClassTerm + ". " + record.PropertyTerm + ". " + record.RepresentationTerm);
            record.Definition.CheckedField("Definition").Must().Not().BeEmpty().And().Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.ObjectClassTerm.AsName().CheckedField("Object Class Term").Must().Be(accName);
            record.PropertyTermQualifiers.CheckedField("Property Term Qualifier(s)").Should().BeEmpty();
            record.PropertyTerm.CheckedField("Property Term").Must().Not().BeEmpty();
            record.DatatypeQualifiers.CheckedField("Data Type Qualifier(s)").Should().BeEmpty();
            record.RepresentationTerm.CheckedField("Representation Term").Must().Not().BeEmpty();
            record.QualifiedDataTypeUID.CheckedField("Qualified Data Type UID").Should().BeEmpty();
            record.AssociatedObjectClassTermQualifiers.CheckedField("Associated Object Class Term Qualifier(s)").Should().BeEmpty();
            record.AssociatedObjectClass.CheckedField("Associated Object Class").Should().BeEmpty();
            record.BusinessTerms.CheckedField("Business Terms").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.UsageRules.CheckedField("Usage Rules").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            //            record.SequenceNumber.CheckedField("Sequence Number");
//            record.OccurrenceMin.CheckedField("Occurrence Min");
//            record.OccurrenceMax.CheckedField("Occurrence Max");
//            record.Version;
        }

        private static string[] ToArray(string field)
        {
            return field.Length > 0 ? new[] {field} : new string[0];
        }

        private static string GetACCName(Record record)
        {
            return record.DictionaryEntryName.WithoutSuffix(". Details").AsName();
        }

        private static void CheckACCRecord(Record record)
        {
//            record.UniqueUNAssignedID;
            record.DictionaryEntryName.CheckedField("Dictionary Entry Name").Should().EndWith(". Details");
            record.Definition.CheckedField("Definition").Must().Not().BeEmpty().And().Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.ObjectClassTerm.AsName().CheckedField("Object Class Term").Should().Be(GetACCName(record));
            record.PropertyTermQualifiers.CheckedField("Property Term Qualifier(s)").Should().BeEmpty();
            record.PropertyTerm.CheckedField("Property Term").Should().BeEmpty();
            record.DatatypeQualifiers.CheckedField("Data Type Qualifier(s)").Should().BeEmpty();
            record.RepresentationTerm.CheckedField("Representation Term").Should().BeEmpty();
            record.QualifiedDataTypeUID.CheckedField("Qualified Data Type UID").Should().BeEmpty();
            record.AssociatedObjectClassTermQualifiers.CheckedField("Associated Object Class Term Qualifier(s)").Should().BeEmpty();
            record.AssociatedObjectClass.CheckedField("Associated Object Class").Should().BeEmpty();
            record.BusinessTerms.CheckedField("Business Terms").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.UsageRules.CheckedField("Usage Rules").Should().Not().BeLongerThan(MaximumTaggedValueLength);
            record.SequenceNumber.CheckedField("Sequence Number").Should().BeEmpty();
            record.OccurrenceMin.CheckedField("Occurrence Min").Should().BeEmpty();
            record.OccurrenceMax.CheckedField("Occurrence Max").Should().BeEmpty();
//            record.Version;
        }

        private static ICDT FindCDT(string name, Dictionary<string, ICDT> cdts)
        {
            ICDT cdt;
            cdts.TryGetValue(name, out cdt);
            return cdt;
        }

        private static Record GetRecord(string line)
        {
            string[] values = line.Split(new[] {'\t'});
            if (values.Length != 18)
            {
                throw new Exception("Invalid number of separators detected: " + values.Length);
            }
            return new Record
                   {
                       UniqueUNAssignedID = values[0],
                       ElementType = values[1],
                       DictionaryEntryName = values[2],
                       Definition = values[3],
                       ObjectClassTerm = values[4],
                       PropertyTermQualifiers = values[5],
                       PropertyTerm = values[6],
                       DatatypeQualifiers = values[7],
                       RepresentationTerm = values[8],
                       QualifiedDataTypeUID = values[9],
                       AssociatedObjectClassTermQualifiers = values[10],
                       AssociatedObjectClass = values[11],
                       BusinessTerms = values[12],
                       UsageRules = values[13],
                       SequenceNumber = values[14],
                       OccurrenceMin = values[15],
                       OccurrenceMax = values[16],
                       Version = values[17],
                   };
        }
    }

    internal class ACCResolver
    {
        private readonly ICCLibrary ccLibrary;
        private Dictionary<string, IACC> accs;

        public ACCResolver(ICCLibrary ccLibrary)
        {
            this.ccLibrary = ccLibrary;
        }

        private IACC GetACC(string name)
        {
            if (accs == null)
            {
                accs = new Dictionary<string, IACC>();
                foreach (IACC acc in ccLibrary.Elements)
                {
                    accs[acc.Name] = acc;
                }
            }
            IACC result;
            if (!accs.TryGetValue(name, out result))
            {
                Console.WriteLine("WARNING: ACC " + name + " not found.");
            }
            return result;
        }

        public Func<IACC> ResolveACC(string name)
        {
            return () => GetACC(name);
        }
    }

    public static class StringExtensions
    {
        public static ExpectationBuilder CheckedField(this string fieldValue, string name)
        {
            return new ExpectationBuilder(fieldValue, name);
        }

        public static string WithoutSuffix(this string str, string suffix)
        {
            return str.EndsWith(suffix) ? str.Remove(str.Length - suffix.Length) : str;
        }

        public static string LimitTo(this string str, int maxLength)
        {
            return str.Length > maxLength ? str.Remove(maxLength) : str;
        }

        public static string AsName(this string str)
        {
            return str.Replace(" ", "").Replace("-", "");
        }
    }

    public class ExpectationBuilder
    {
        private readonly string fieldName;
        private readonly string fieldValue;
        private ErrorLevel errorLevel = ErrorLevel.Warning;
        private bool expected = true;

        public ExpectationBuilder(string fieldValue, string fieldName)
        {
            this.fieldValue = fieldValue;
            this.fieldName = fieldName;
        }

        public ExpectationBuilder Should()
        {
            errorLevel = ErrorLevel.Warning;
            return this;
        }

        public ExpectationBuilder Must()
        {
            errorLevel = ErrorLevel.Error;
            return this;
        }

        public ExpectationBuilder Not()
        {
            expected = !expected;
            return this;
        }

        public ExpectationBuilder BeEmpty()
        {
            Check(fieldValue.Length == 0, "be empty");
            return this;
        }

        private void Check(bool expectation, string description, params object[] args)
        {
            if (expected != expectation)
            {
                PrintWarning(description, args);
            }
        }

        private void PrintWarning(string expectation, params object[] args)
        {
            Console.WriteLine("{0}: Line {1}: Field <{2}> {4} {5}, but is <{3}>.", errorLevel.ToString().ToUpper(),
                              Program.LineNumber,
                              fieldName,
                              fieldValue,
                              GetExpectationKind(), string.Format(expectation, args));
        }

        private string GetExpectationKind()
        {
            switch (errorLevel)
            {
                case ErrorLevel.Error:
                    return "must" + (expected ? "" : " not");
                default:
                    return "should" + (expected ? "" : " not");
            }
        }

        public ExpectationBuilder EndWith(string suffix)
        {
            Check(fieldValue.EndsWith(suffix), "end with <{0}>", suffix);
            return this;
        }

        public ExpectationBuilder Be(string expectedValue)
        {
            Check(fieldValue == expectedValue, "be <{0}>", expectedValue);
            return this;
        }

        public ExpectationBuilder And()
        {
            return new ExpectationBuilder(fieldValue, fieldName);
        }

        public ExpectationBuilder BeLongerThan(int maximumLength)
        {
            Check(fieldValue.Length > maximumLength, "be longer than {0} characters, but is {1} characters long", maximumLength, fieldValue.Length);
            return this;
        }
    }

    internal enum ErrorLevel
    {
        Warning,
        Error
    }

    public struct Record
    {
        public string AssociatedObjectClass;
        public string AssociatedObjectClassTermQualifiers;
        public string BusinessTerms;
        public string DatatypeQualifiers;
        public string Definition;
        public string DictionaryEntryName;
        public string ElementType;
        public string ObjectClassTerm;
        public string OccurrenceMax;
        public string OccurrenceMin;
        public string PropertyTerm;
        public string PropertyTermQualifiers;
        public string QualifiedDataTypeUID;
        public string RepresentationTerm;
        public string SequenceNumber;
        public string UniqueUNAssignedID;
        public string UsageRules;
        public string Version;
    }
}