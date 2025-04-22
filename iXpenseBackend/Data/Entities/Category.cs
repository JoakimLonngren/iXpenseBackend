using System.ComponentModel.DataAnnotations;

namespace iXpenseBackend.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
