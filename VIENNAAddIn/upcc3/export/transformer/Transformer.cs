using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.transformer
{
    public static class Transformer
    {
        public static void Transform(IDocLibrary sourceLibrary, IDocLibrary targetLibrary)
        {
            // ************** ANSATZ 1 **************************            
            // 0. BDTs und ASBIEs werden derzeit ignoriert.
            
            // 1. Hashset von allen BCCs => dazu muessen wir:
            //    => in der source BIE (i.e. ebInterface) library alle ABIEs durchlaufen, darin die BBIEs durchlaufen, 
            //       und fuer jeden BBIE den basedOn BCC im Hashset speichern

            // 2. Durchlaufen aller ABIEs in der BIE Library von target (i.e. UBL), fuer jeden ABIE die BBIEs durchlaufen und
            //    fuer jeden BBIE folgenden Schritt durchführen
            //    => ist der BCC, auf welchem der BBIE basiert im Hashset vorhanden?
            //         nein => loeschen des BBIEs
            //         ja => nix tun
            //
            //    The same alrogithm is used for BDT/SUP as well as ASBIEs.

            // 3. Durchlaufen des DOC Library Baumes, und bei den Blättern beginnend: existiert ein BBIE oder an ASBIE innherhalb
            //    des aktuellen ABIEs? 
            //
            //    Nein? Dann wird der ABIE gelöscht. Es müssen auch die ASMAs oder ASBIEs welche auf den ABIE zeigen, entfernt werden.

            // ************** ANSATZ 2 **************************
            // Prinzipiell ist der Algorithmus gleich nur dass in diesem Ansatz nicht ueber die BIE Libraries iteriert wird
            // sondern rekursiv ueber die DOC Baeume.
        }
    }
}