using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public interface IArchiveContext : IPage
    {
        ArchiveContextSettings Archives { get; set; }
    }
    public class ArchiveContextSettings
    {
        public string Directory { get; set; }
        public bool AppendChildPath { get; set; }
    }
}
