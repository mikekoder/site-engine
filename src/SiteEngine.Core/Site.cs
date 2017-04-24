using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteEngine
{
    public class Site
    {
        public IEnumerable<Page> Pages
        {
            get
            {
                return pages.Where(p => p.Published <= DateTime.Now);
            }
        }

        private string directory;
        private IEnumerable<Type> pageTypes;

        private List<Page> pages = new List<Page>();
        private object syncLock = new object();
        private FileSystemWatcher watcher;
        public Site(string directory, IEnumerable<Type> pageTypes)
        {
            this.directory = directory;
            this.pageTypes = pageTypes;
            watcher = new FileSystemWatcher(directory);
            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Changed;
            watcher.Deleted += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            LoadPages();
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadPages();
        }
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            LoadPages();
        }

        public Page GetLastPageInPath(string path)
        {
            var segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            for(var i = segments.Length; i > 0; i--)
            {
                var page = pages.SingleOrDefault(p => p.Path == "/" + string.Join("/",segments.Take(i)) + "/");
                if(page != null)
                {
                    return page;
                }
            }
            return pages.SingleOrDefault(p => p.Path == "/");
        }
        private void LoadPages()
        {
            var dir = new DirectoryInfo(directory);
            var pages = new List<Page>();
            foreach (var file in dir.EnumerateFiles("*.cshtml", SearchOption.AllDirectories))
            {
                var page = MakePage(file);
                if (page != null && !page.Draft)
                {
                    pages.Add(page);
                }
            }

            
            BuildPages(pages, null);

            var rootPages = pages.Where(p => p.Path == "/").ToList();
            lock (syncLock)
            {
                this.pages = pages;
            }
        }
        private Page MakePage(FileInfo file)
        {
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                string line;
                bool readingJson = false;
                bool jsonRead = false;
                var buffer = new StringBuilder();
                string type = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var modelIndex = line.IndexOf("@model");
                    if (modelIndex >= 0)
                    {
                        type = line.Substring(modelIndex + 6).Trim();
                    }

                    if (!jsonRead && !readingJson)
                    {        
                        var index = line.IndexOf("@*");
                        if (index >= 0)
                        {
                            buffer.AppendLine(line.Substring(index + 2));
                            readingJson = true;
                        }
                    }
                    else if(readingJson)
                    {
                        var index = line.IndexOf("*@");
                        if (index >= 0)
                        {
                            buffer.AppendLine(line.Substring(0, index));
                            jsonRead = true;
                            readingJson = false;
                        }
                        else
                        {
                            buffer.AppendLine(line);
                        }
                    }
                }
                var json = buffer.ToString().Trim();
                if(json.Length == 0)
                {
                    return null;
                }

                if (!json.StartsWith("{"))
                {
                    json = "{" + json;
                }
                if (!json.EndsWith("}"))
                {
                    json += "}";
                }

                var pageType = !string.IsNullOrWhiteSpace(type) ? pageTypes.SingleOrDefault(t => t.FullName.EndsWith(type)) : typeof(Page);
                var page = JsonConvert.DeserializeObject(json, pageType ?? typeof(Page)) as Page;
                page.ViewPath = "~/" + directory.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last() + file.FullName.Substring(directory.Length).Replace('\\','/');
                return page;
            }
        }
        private void BuildPages(IEnumerable<Page> pages, Page parent)
        {
            if(parent == null)
            {
                foreach (var page in pages.Where(p => string.IsNullOrEmpty(p.ParentId)))
                {
                    page.Parent = parent;
                    BuildPages(pages, page);
                }
            }
            else
            {
                foreach (var page in pages.Where(p => p.ParentId == parent.Id))
                {
                    page.Parent = parent;
                    BuildPages(pages, page);
                }
            }
            
        }
    }
}
