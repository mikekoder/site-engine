using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public abstract class ViewPageBase<TModel> : RazorPage<TModel>
    {
        [RazorInject]
        public Site Site { get; set; }
        public object SubContent
        {
            get
            {
                return ViewData["SubModel"];
            }
        }
    }
}
