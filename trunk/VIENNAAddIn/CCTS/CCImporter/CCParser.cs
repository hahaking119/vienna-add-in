using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using VIENNAAddIn.common.corecomponents;

namespace VIENNAAddIn.CCTS.CCLImporter
{



    public class CCParser
    {



        /// <summary>
        /// Parser
        /// </summary>
        public List<ACC> parse(String file)
        {

            List<ACC> accs = new List<ACC>();

            Console.WriteLine("Starting CC Parser....");

            //Open the file
            StreamReader reader = File.OpenText(file);
            String s = null;
            ACC acc = null;

            int lineNumber = 0;


            while ((s = reader.ReadLine()) != null)
            {
                lineNumber++;

                //Ignore the first 
                if (lineNumber < 2)
                    continue;

                //Split the input string
                char[] separators = { ';' };
                String[] values = s.Split(separators);

                //Throw an exception if there is a separator mismatch
                if (values.Length != 59)
                    throw (new Exception("Invalid number of separators detected: " + values.Length + " Line number: " + lineNumber));

                //Column 2 tells us what we have (ACC/BCC or ASCC)
                if (values[2] == "ACC")
                {
                    acc = new ACC();

                    acc.Definition = values[4].Trim();
                    acc.ObjectClassTerm = values[8].Trim();
                    acc.PropertyTerm = values[10].Trim();
                    acc.RepresentationTerm = values[12].Trim();

                    //Add the acc to the list
                    accs.Add(acc);
                }
                else if (values[2] == "BCC")
                {
                    BCC bcc = new BCC();
                    bcc.Definition = values[4].Trim();
                    bcc.ObjectClassTerm = values[8].Trim();
                    bcc.PropertyTerm = values[10].Trim();
                    bcc.RepresentationTerm = values[12].Trim();

                    //Set upper and lower bounds for the multiplicity
                    bcc.LowerBoundMultiplicity = values[19].Trim();
                    bcc.UpperBoundMultiplicity = values[20].Trim();

                    //a BCC always belongs to an acc - hence save it to the
                    //appropriate one
                    acc.addBCC(bcc);
                }
                else
                {
                    ASCC ascc = new ASCC();

                    ascc.AssociatedObjectClass = values[15].Trim();

                    //Set upper and lower bounds for the multiplicity
                    ascc.LowerBoundMultiplicity = values[19].Trim();
                    ascc.UpperBoundMultiplicity = values[20].Trim();
                    ascc.PropertyTerm = values[10].Trim();

                    acc.addASCC(ascc);

                }


            }
            reader.Close();


            Console.WriteLine("Finished CC Parser successfully.");
            Console.WriteLine("Parsed " + accs.Count + " ACCs in total.");

            return accs;

        }



























    }













}
