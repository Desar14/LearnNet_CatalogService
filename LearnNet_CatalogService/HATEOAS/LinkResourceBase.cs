﻿namespace LearnNet_CatalogService.HATEOAS
{
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {

        }

        public List<Link> Links { get; set; } = new List<Link>();
    }
}