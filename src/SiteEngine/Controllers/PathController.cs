using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class PathController : Controller
    {
        private readonly Site repository;
             
        public PathController(Site repository)
        {
            this.repository = repository;
        }
        public IActionResult Resolve(string path)
        {
            path = path ?? "/";
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            var page = repository.GetLastPageInPath(path);
            if(page == null)
            {
                return View("NotFound");
            }
            if(page.Path != path)
            {
                var relativePath = "/" + path.Substring(page.Path.Length);
                var subContent = page.GetSubModel(relativePath, repository);
                ViewData["SubModel"] = subContent;
            }
            return View(page.ViewPath, page);
            
        }
    }
}