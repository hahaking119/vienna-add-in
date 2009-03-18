using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using VIENNAAddIn.common.corecomponents;

namespace VIENNAAddIn.CCTS.CCLImporter
{
    class CDTParser
    {





        /// <summary> 
        /// Parser
        /// </summary>
        public List<CDT> parse(String file)
        {

            List<CDT> cdts = new List<CDT>();

            Console.WriteLine("Starting CDT Parser....");

            //Open the file
            StreamReader reader = File.OpenText(file);
            String s = null;
            CDT cdt = null;
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
                if (values.Length != 78)
                    throw (new Exception("Invalid number of separators detected: " + values.Length + " Line number: " + lineNumber));

                //Column 2 tells us what we have (DT/CC/SC)
                if (values[2] == "DT")
                {

                    cdt = new CDT();
                    cdt.ObjectClassTerm = values[12].Trim();

                    //Add the cdt to the list
                    cdts.Add(cdt);
                }
                else if (values[2] == "CC")
                {
                    ContentComponent con = new ContentComponent();
                    con.Definition = values[4].Trim();

                    //Since the input file is unfortunately incomplete we have to split up
                    //the Dictionary Entry Name in order to get 
                    //object class term
                    char[] sep = { '.' };
                    String[] subvalues = values[3].Split(sep);

                    try
                    {
                        con.ObjectClassTerm = subvalues[0].Trim();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in line " + lineNumber + " with value " + values[2]);
                        throw (e);

                    }


                    //A CDT has exactly one CON
                    cdt.Con = con;
                }
                else
                {
                    SupplementaryComponent sup = new SupplementaryComponent();
                    sup.Definition = values[4].Trim();

                    //Since the input file is unfortunately incomplete we have to split up
                    //the Dictionary Entry Name in order to get 
                    //object class term
                    //property term
                    //and representation term
                    char[] sep = { '.' };
                    String[] subvalues = values[3].Split(sep);

                    try
                    {

                        sup.ObjectClassTerm = subvalues[0].Trim();
                        sup.PropertyTerm = subvalues[1].Trim();

                        //RepresentationTerm is not always provided
                        if (subvalues.Length > 2)
                            sup.RepresentationTerm = subvalues[2].Trim();
                        else
                            sup.RepresentationTerm = subvalues[1].Trim();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in line " + lineNumber + " with value " + values[3].Trim());
                        throw (e);
                    }

                    //Set the sequence number
                    sup.SequenceNumber = values[11].Trim();


                    cdt.addSup(sup);

                }


            }
            reader.Close();


            Console.WriteLine("Finished CDT Parser successfully.");
            Console.WriteLine("Parsed " + cdts.Count + " CDTs in total.");

            return cdts;

        }



































    }
}
