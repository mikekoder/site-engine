using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public interface ITagContext : IPage
    {
        TagContextSettings Tagging { get; set; }
    }
    public class TagContextSettings
    {
        public string Directory { get; set; }
    }
}
