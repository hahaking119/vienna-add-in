using System;
using System.Collections.Generic;

namespace CctsRepository.DocLibrary
{
    public class MaSpec
    {
        public string Name { get; set; }

        public IEnumerable<AsmaSpec> Asmas { get; set; }
    }
}