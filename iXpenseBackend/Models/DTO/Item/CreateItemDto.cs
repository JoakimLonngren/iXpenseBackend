using System.ComponentModel.DataAnnotations;

namespace iXpenseBackend.Models.DTO.Item
{
    public class CreateItemDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title {  get; set; }


        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Quantity of this item must be greater than zero.")]
        public int Quantity { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be valid.")]
        public int CategoryId { get; set; }
    }
}
