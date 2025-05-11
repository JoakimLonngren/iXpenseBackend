using iXpenseBackend.Models.DTO.Item;
using System.ComponentModel.DataAnnotations;

namespace iXpenseBackend.Models.DTO.Receipt
{
    public class CreateReceiptDto
    {
        [Required(ErrorMessage = "From field required")]
        public string From { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date {  get; set; }

        [MinLength(1, ErrorMessage = "A receipt must contain atleast one product.")]
        public List<CreateItemDto> Items { get; set; } = new List<CreateItemDto>();
    }
}
