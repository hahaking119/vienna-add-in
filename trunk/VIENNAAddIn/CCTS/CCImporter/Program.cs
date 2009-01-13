using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VIENNAAddIn.common.corecomponents;


namespace VIENNAAddIn.CCTS.CCLImporter
{
    public class Program
    {


        static String CCFILE = "C:\\VIENNAAddIn\\VIENNAAddIn\\CCTS\\CCLImporter\\input\\CCL08A_CC.csv";
        static String CDTFILE = "C:\\VIENNAAddIn\\VIENNAAddIn\\CCTSCCLImporter\\input\\CCL08A_CDT.csv";

        static String QDTFILE = "C:\\VIENNAAddIn\\VIENNAAddIn\\CCTSCCLImporter\\input\\CCL08A_QDT.csv";
        static String BIEFILE = "C:\\VIENNAAddIn\\VIENNAAddIn\\CCTSCCLImporter\\input\\CCL08A_BIE.csv";

        static String TARGETFILE = "C:\\VIENNAAddIn\\VIENNAAddIn\\CCTSCCLImporter\\input\\CCL08A.eap";

        static String LIBRARYVERSIOPN = "CCL08A";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            DateTime start = DateTime.Now;
            try
            {


                ////Start the CCParser
                CCParser parser = new CCParser();
                List<ACC> accs = null;
                accs = parser.parse(CCFILE);

                ////Start the CDTParser
                CDTParser cdtParser = new CDTParser();
                List<CDT> cdts = null;
                cdts = cdtParser.parse(CDTFILE);

                //Start the QDT Parser
                QDTParser qdtParser = new QDTParser();
                List<QDT> qdts = qdtParser.parse(QDTFILE);

                //Start the BIEParser
                BIEParser bieParser = new BIEParser();
                List<ABIE> abies = bieParser.parse(BIEFILE);



                EAModelWriter writer = new EAModelWriter(TARGETFILE, LIBRARYVERSIOPN);

                //Start the EAFile-Writer for CDT
                Dictionary<string, EA.Element> savedCDTs = null;
                savedCDTs = writer.writeCDT(cdts);

                //Start the EAFile-Writer for QDT
                Dictionary<string, EA.Element> savedQDTs = writer.writeQDT(qdts, savedCDTs);

                //Start the EAFile-Writer for CC                
                Dictionary<string, EA.Element> savedCCs = null;
                savedCCs = writer.writeCC(accs, savedCDTs);

                //Start the EAFile-Writer for BIE
                writer.writeBIE(abies, savedQDTs, savedCCs);



                Console.WriteLine("******************************************");
                Console.WriteLine("Successfully completed parsing and writing");

                printElapsedTime(start);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                Console.WriteLine("Exit -1.");
                printElapsedTime(start);
            }
        }



        private static void printElapsedTime(DateTime start)
        {
            TimeSpan elapsed = DateTime.Now - start;
            Console.WriteLine("Process finished after: " + (Int32)elapsed.TotalMinutes + " min. " + (Int32)elapsed.TotalSeconds + " sec.");
        }








    }
}
