using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iXpenseBackend.Data.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; } = null!;
    }
}
