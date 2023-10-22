namespace LearnNet_CatalogService.HATEOAS
{
    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public List<T> Values { get; set; } = new List<T>();

        public LinkCollectionWrapper()
        {

        }

        public LinkCollectionWrapper(List<T> value)
        {
            Values = value;
        }
    }
}
