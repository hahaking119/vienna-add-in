using System;
using EA;

public class SynchStereotypes
{
    public SynchStereotypes()
	{
	}

    public ArrayList<String> Check(String stereotype, Package p)
    {
        switch (stereotype)
        {
            case "ABIE":
                foreach (Element e in p.Elements)
                    if (e.Stereotype.equals("ABIE") CheckABIE(e);
                foreach (Package pp in p.Packages)
                    Check("ABIE", pp);
                break;
            case "ACC":
                foreach (Element e in p.Elements)
                    if (e.Stereotype.equals("ACC") CheckACC(e);
                foreach (Package pp in p.Packages)
                    Check("ACC", pp);
                break;
            //...
            case "all":
                foreach (Element e in p.Elements)
                    switch (e.Stereotype)
                    {
                        case "ABIE":
                            CheckABIE(e);
                            break;
                        case "ACC":
                            CheckACC(e);
                            break;
                        //..
                    }
                    foreach (Package pp in p.Packages)
                        Check("all", pp);
                break;
        }
    }

    public void Fix(String Stereotype)
    {
        // copy Check method
    }

    private ArrayList<String> CheckABIE(Element e)
    {
        var taggedValues = new ArrayList<String>();
        var missingValues = new ArrayList<String>();
        taggedValues.AddRange(new string[] { "definition", "dictionaryEntryName" });

        foreach (string s in taggedValues)
        {
            if (e.GetTaggedValues(this, s) == "?")
                missingValues.Add(s);
        }
    }

    private ArrayList<String> CheckACC(Element e)
    {
        var taggedValues = new ArrayList<String>();
        var missingValues = new ArrayList<String>();
        taggedValues.AddRange(new string[] { "definition", "dictionaryEntryName" });

        foreach (string s in taggedValues)
        {
            if (e.GetTaggedValues(this, s) == "?")
                missingValues.Add(s);
        }
    }

    private bool FixABIE(Element e, ArrayList<String> missingValues)
    {
        foreach (string s in missingValues)
        {
            // add to TaggedValues
        }
    }

    private bool FixACC(Element e, ArrayList<String> missingValues)
    {
        foreach (string s in missingValues)
        {
            // add to TaggedValues
        }
    }
}

/*
        ABIE,
        ACC,
        ASBIE,
        ASCC,
        BBIE,
        BCC,
        BCSS,
        BIE,
        BIELibrary,
        bLibrary,
        BusinessLibrary,
        CC,
        CCLibrary,
        CCTS,
        CDT,
        CDTLibrary,
        CON,
        DOCLibrary,
        ENUM,
        ENUMLibrary,
        PRIM,
        PRIMLibrary,
        QDT,
        QDTLibrary,
        SUP,
        basedOn,
        BDT
*/
