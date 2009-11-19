using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    ///<typeparam name="TICCTSElement"></typeparam>
    ///<typeparam name="TCCTSElement"></typeparam>
    ///<typeparam name="TCCTSElementSpec"></typeparam>
    public abstract class ElementLibrary<TICCTSElement, TCCTSElement, TCCTSElementSpec> : BusinessLibrary, IElementLibrary<TICCTSElement, TCCTSElementSpec>
        where TCCTSElement : UpccClass<TCCTSElementSpec>, TICCTSElement
        where TCCTSElementSpec : CCTSElementSpec
    {
        private static readonly string ElementStereotype = CctsRepository.Stereotype.GetStereotype<TICCTSElement>();

        protected ElementLibrary(CCRepository repository, Package package) : base(repository, package)
        {
        }

        public TICCTSElement ElementByName(string name)
        {
            foreach (TICCTSElement element in Elements)
            {
                if (((TCCTSElement)element).Name == name)
                {
                    return element;
                }

            }
            return default(TICCTSElement);
        }

        public IEnumerable<TICCTSElement> Elements
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsA(ElementStereotype))
                    {
                        yield return CreateCCTSElement(element);
                    }
                }
            }
        }

        protected abstract TCCTSElement CreateCCTSElement(Element element);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ///<param name="spec"></param>
        ///<returns></returns>
        public TICCTSElement CreateElement(TCCTSElementSpec spec)
        {
            var element = (Element)package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = ElementStereotype;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            var cctsElement = CreateCCTSElement(element);
            cctsElement.Update(spec);
            return cctsElement;
        }

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ///<param name="element"></param>
        ///<param name="spec"></param>
        ///<returns></returns>
        public TICCTSElement UpdateElement(TICCTSElement element, TCCTSElementSpec spec)
        {
            ((TCCTSElement)element).Update(spec);
            return element;
        }

    }
}