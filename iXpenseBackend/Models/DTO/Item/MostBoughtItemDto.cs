namespace iXpenseBackend.Models.DTO.Item
{
    public class MostBoughtItemDto
    {
        public string ItemTitle { get; set; } = null!;
        public string CategoryTitle { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AveragePrice { get; set; }

    }
}
