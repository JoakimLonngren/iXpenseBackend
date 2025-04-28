using iXpenseBackend.Models.DTO.Item;

namespace iXpenseBackend.Models.DTO.Receipt
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public string From { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ItemDto> Items { get; set; }
    }
}
