using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace iXpenseBackend.Data.Entities
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date {  get; set; } 
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
