using System;
using System.Collections.Generic;
using System.Text;
using VIENNAAddIn.common.corecomponents;
using System.IO;


namespace VIENNAAddIn.CCTS.CCLImporter
{





    public class BIEParser
    {



        /// <summary>
        /// Parser
        /// </summary>
        public List<ABIE> parse(String file)
        {

            List<ABIE> abies = new List<ABIE>();

            Console.WriteLine("Starting ABIE Parser....");

            //Open the file
            StreamReader reader = File.OpenText(file);
            String s = null;
            ABIE abie = null;
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

                //Column 2 tells us what we have (ABIE, BBIE, ASBIE)
                if (values[2] == "ABIE")
                {
                    abie = new ABIE();

                    abie.Definition = values[4].Trim();

                    abie.ObjectClassTermQualifier = values[7].Trim();
                    abie.ObjectClassTerm = values[8].Trim();

                    abie.PropertyTerm = values[10].Trim();
                    abie.RepresentationTerm = values[12].Trim();

                    //Add the acc to the list
                    abies.Add(abie);
                }
                else if (values[2] == "BBIE")
                {
                    BBIE bbie = new BBIE();
                    bbie.Definition = values[4].Trim();

                    bbie.ObjectClassTermQualifier = values[7].Trim();
                    bbie.ObjectClassTerm = values[8].Trim();

                    bbie.PropertyTermQualifier = values[9].Trim();
                    bbie.PropertyTerm = values[10].Trim();

                    bbie.RepresentationTerm = values[12].Trim();

                    //Set upper and lower bounds for the multiplicity
                    bbie.LowerBoundMultiplicity = values[19].Trim();
                    bbie.UpperBoundMultiplicity = values[20].Trim();

                    //a BCC always belongs to an acc - hence save it to the
                    //appropriate one
                    abie.addBBIE(bbie);
                }
                else
                {
                    ASBIE asbie = new ASBIE();
                    asbie.Definition = values[4].Trim();


                    asbie.AssociatedObjectClassTermQualifier = values[14].Trim();
                    asbie.AssociatedObjectClass = values[15].Trim();
                    asbie.PropertyTerm = values[10].Trim();

                    //Set upper and lower bounds for the multiplicity
                    asbie.LowerBoundMultiplicity = values[19].Trim();
                    asbie.UpperBoundMultiplicity = values[20].Trim();

                    abie.addASBIE(asbie);


                }


            }
            reader.Close();


            Console.WriteLine("Finished BIE Parser successfully.");
            Console.WriteLine("Parsed " + abies.Count + " ABIEs in total.");

            return abies;

        }

















    }
}
