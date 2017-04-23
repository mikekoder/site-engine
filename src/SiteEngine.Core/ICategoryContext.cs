using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public interface ICategoryContext : IPage
    {
        CategoryContextSettings Categorization { get; set; }
    }
    public class CategoryContextSettings
    {
        public string Directory { get; set; }
    }
}
