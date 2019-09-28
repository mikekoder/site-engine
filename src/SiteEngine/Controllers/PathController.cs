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
            path ??= "/";
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
                bool resolved = false;
                object subContent = null;
                var relativePath = "/" + path.Substring(page.Path.Length);

                
                if (!resolved && page is ICategoryContext && (page as ICategoryContext).TryResolveCategoryPath(relativePath, repository, out subContent))
                {
                    resolved = true;
                }
                if (!resolved && page is ITagContext && (page as ITagContext).TryResolveTagPath(relativePath, repository, out subContent))
                {
                    resolved = true;
                }
                if (!resolved && page is IArchiveContext && (page as IArchiveContext).TryResolveArchivePath(relativePath, repository, out subContent))
                {
                    resolved = true;
                }

                if (!resolved)
                {
                    return View("NotFound");
                }

                ViewData["SubModel"] = subContent;
            }
            return View(page.ViewPath, page);
            
        }
    }
}