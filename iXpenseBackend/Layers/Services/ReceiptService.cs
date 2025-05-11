using iXpenseBackend.Data.Entities;
using iXpenseBackend.Layers.Repositories;
using iXpenseBackend.Models.DTO.Item;
using iXpenseBackend.Models.DTO.Receipt;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace iXpenseBackend.Layers.Services
{
    public class ReceiptService
    {
        private readonly ReceiptRepo _receiptRepo;
        private readonly CategoryService _categoryService;

        public ReceiptService(ReceiptRepo receiptRepo, CategoryService categoryService)
        {
            _receiptRepo = receiptRepo;
            _categoryService = categoryService;
        }

        public async Task<(bool isSuccess, string message)> AddReceiptAsync(CreateReceiptDto createReceiptDto, string userId)
        {
            try
            {
                var items = await MapItems(createReceiptDto.Items);
                var receipt = MapToReceipt(createReceiptDto, items, userId);

                await _receiptRepo.AddReceiptAsync(receipt);
                return (true, "Receipt created succesfully.");
            }
            catch (Exception ex)
            {
                return (false, "An error occured while trying to add receipt.");
            }
        }

        public async Task<(bool isSuccess, string message)> DeleteReceiptAsync(int id, string userId)
        {
            try
            {
                var receipt = await _receiptRepo.GetReceiptByIdAndUserIdAsync(id, userId);
                if(receipt == null)
                {
                    return (false, "Receipt was not found or belongs to another user.");
                }

                var isDeleted = await _receiptRepo.DeleteReceiptAsync(receipt);
                return isDeleted
                    ? (true, "Receipt was successfully deleted")
                    : (false, "An error occured while trying to remove this receipt.");
            }
            catch (Exception ex)
            {
                return (false, "An error occued while deleting this receipt.");
            }
        }

        public async Task<(bool isSuccess, string message, IEnumerable<ReceiptDto>? data)> GetUserReceiptAsync(string userId)
        {
            var receipts = await _receiptRepo.GetReceiptByUserIdAsync(userId);
            if(receipts == null || !receipts.Any())
            {
                return (false, "You have not registered any receipts.", null);
            }

            var receiptsDto = receipts.Select(r => new ReceiptDto
            {
                Id = r.Id,
                From = r.From,
                TotalAmount = r.TotalAmount,
                Date = r.Date,
                Items = r.Items.Select(i => new ItemDto
                {
                    Title = i.Title,
                    Price = i.Price,
                    Quantity = i.Quantity,
                }).ToList()
            });
            return (true, "Receipts retrieved successfully", receiptsDto);
        }

        public async Task <(bool isSuccess, string message, MostBoughtItemDto? data)> GetMostPurchasedItemAsync(string userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return (false, "Couldn´t find a userId.", null);
                }

                if(startDate >  endDate)
                {
                    return (true, "Start date cant be later than end date.", null);
                }

                var mostBoughtItem = await _receiptRepo.GetMostPurchasedItemAsync(userId, startDate, endDate);

                if(mostBoughtItem == null)
                {
                    return (true, "You have no registered purchases during the selected period.", null);
                }

                return (true, "Top purchased item retrieved successfully.", mostBoughtItem);
            }
            catch (Exception ex)
            {
                return (false, "An unexpected error occured while retrieving the information.", null);
            }
        }

        public async Task <(bool isSuccess, string message, List<MostBoughtCategoryDto>? data)> GetMostPurchasedCategoryAsync(string userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return (false, "User ID is required", null);

                if (startDate > endDate)
                    return (true, "Startdate cannot be later than End-date", null);

                var result = await _receiptRepo.GetMostPurchasedCategoryAsync(userId, startDate, endDate);

                if (result == null || !result.Any())
                    return (true, "No categories found during the selected period.", null);

                return (true, "Category spending retrieved successfully.", result);
            }
            catch (Exception)
            {
                return (false, "An error occured while retrieving the categories", null);
            }
        }


        private async Task<List<Item>> MapItems(List<CreateItemDto> itemsDto)
        {
            var items = new List<Item>();

            foreach (var createItemDto in itemsDto)
            {
                var category = await _categoryService.GetCategoryByIdAsync(createItemDto.CategoryId);
                if (category == null)
                {
                    throw new Exception($"Category with ID {createItemDto.CategoryId} does not exist.");
                }

                items.Add(new Item
                {
                    Title = createItemDto.Title,
                    Price = createItemDto.Price,
                    Quantity = createItemDto.Quantity,
                    Category = category
                });
            }
            return items;
        }
        
        private Receipt MapToReceipt(CreateReceiptDto dto, List<Item> items, string userId)
        {
            return new Receipt
            {
                From = dto.From,
                Date = dto.Date,
                TotalAmount = items.Sum(item => item.Price * item.Quantity),
                UserId = userId,
                Items = items
            };
        }
    }
}
