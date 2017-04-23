using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.Models
{
    public class Blog : Page, IArchiveContext, ICategoryContext, ITagContext
    {
        public ArchiveContextSettings Archives { get; set; }
        public CategoryContextSettings Categorization { get; set; }
        public TagContextSettings Tagging { get; set; }
    }
}
