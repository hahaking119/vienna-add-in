using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

public class SynchStereotypes
{
    public SynchStereotypes()
	{
	}

    public List<String> Check(String stereotype, Package p)
    {
        switch (stereotype)
        {
            case "ABIE":
                foreach (Element e in p.Elements)
                    if (e.Stereotype == "ABIE") CheckABIE(e);
                foreach (Package pp in p.Packages)
                    Check("ABIE", pp);
                break;
            case "ACC":
                foreach (Element e in p.Elements)
                    if (e.Stereotype=="ACC") CheckACC(e);
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

    private List<String> CheckABIE(Element e)
    {
        var missingValues = new List<String>();
            if (e != null)
                if (e.GetTaggedValue(TaggedValues.Definition) == null)
                    missingValues.Add(TaggedValues.Definition.AsString());
                if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                    missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
        return missingValues;
    }


    private List<String> CheckACC(Element e)
    {
        var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

        return missingValues;
    }

    private bool FixABIE(Element e, ArrayList<String> missingValues)
    {
        foreach (string s in missingValues)
        {
            // add to TaggedValues
        }
        return false;
    }

    private bool FixACC(Element e, ArrayList<String> missingValues)
    {
        foreach (string s in missingValues)
        {
            // add to TaggedValues
        }
        return false;
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
