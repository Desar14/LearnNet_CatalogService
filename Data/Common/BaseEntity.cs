using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnNet_CatalogService.Data.Common
{
    public class BaseEntity<T> where T : struct
    {
        public T Id { get; set; }
    }
}
