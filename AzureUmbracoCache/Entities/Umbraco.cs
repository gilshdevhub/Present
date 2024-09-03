using System;
using System.Collections.Generic;

namespace AzureUmbracoCache.Entities.FilteredContent
{
    public class FilteredContent
    {
        public FilteredContent(Guid id, string name, IDictionary<string, object> properties, string url)
        {
            Id = id;
            Name = name;
            Properties = properties;
            Url = url;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IDictionary<string,object> Properties { get; set; }
        public string Url { get; set; }
    }
}
