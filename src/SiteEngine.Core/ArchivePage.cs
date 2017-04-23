using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public class ArchivePage : FilteredChildrenPage
    {
        public int Year { get; set; }
        public int? Month { get; set; }
    }
}
