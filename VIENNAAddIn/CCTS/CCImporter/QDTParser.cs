using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using VIENNAAddIn.common.corecomponents;

namespace VIENNAAddIn.CCTS.CCLImporter
{

    public class QDTParser
    {



        /// <summary> 
        /// Parser
        /// </summary>
        public List<QDT> parse(String file)
        {

            List<QDT> qdts = new List<QDT>();

            Console.WriteLine("Starting QDT Parser....");

            //Open the file
            StreamReader reader = File.OpenText(file);
            String s = null;
            QDT qdt = null;
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

                //Column 1 tells us what we have (DT/CC/SC)
                if (values[2] == "DT")
                {

                    qdt = new QDT();
                    qdt.ObjectClassTerm = values[3].Trim();
                    qdt.RepresentationTerm = values[12].Trim();
                    qdt.Definition = values[4].Trim();

                    qdt.DataTypeQualifier = values[11].Trim();
                    qdt.UnderlyingCDT = values[12].Trim();

                    //Add the cdt to the list
                    qdts.Add(qdt);
                }
                else if (values[2] == "CC")
                {
                    ContentComponent con = new ContentComponent();
                    con.Definition = values[4].Trim();

                    con.ObjectClassTerm = values[3].Substring(0, values[3].IndexOf('.')).Trim();

                    //A QDT has exactly one CON
                    qdt.Con = con;
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
                        Console.WriteLine("Error in line " + lineNumber + " with value " + values[2]);
                        throw (e);
                    }
                    qdt.addSup(sup);

                }


            }
            reader.Close();


            Console.WriteLine("Finished QDT Parser successfully.");
            Console.WriteLine("Parsed " + qdts.Count + " QDTs in total.");

            return qdts;

        }

    }
}
