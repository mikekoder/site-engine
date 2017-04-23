using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public class Page
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        [JsonProperty("path")]
        public string StaticPath { get; set; }
        [JsonProperty("parent")]
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string[] Tags { get; set; }
        public string[] Categories { get; set; }
        [JsonIgnore]
        public Page Parent { get; internal set; }
        [JsonIgnore]
        public string Path
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(StaticPath))
                {
                    return StaticPath;
                }
                else if(Parent != null)
                {
                    return $"{Parent.Path}{Slug}/";

                }
                else if (!string.IsNullOrWhiteSpace(Slug))
                {
                    return $"/{Slug}/";
                }
                else
                {
                    return "/";
                }
            }
        }
        [JsonIgnore]
        public string ViewPath { get; internal set; }
        public int? MenuPosition { get; set; }
        public string Excerpt { get; set; }
        public bool Draft { get; set; }

        public virtual object GetSubModel(string relativePath, Site site)
        {
            return null;
        }
    }
}
