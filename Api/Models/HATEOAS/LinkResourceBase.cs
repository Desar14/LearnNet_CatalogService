namespace LearnNet_CatalogService.Api.Models.HATEOAS
{
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {

        }

        public List<Link> Links { get; set; } = new List<Link>();
    }
}
