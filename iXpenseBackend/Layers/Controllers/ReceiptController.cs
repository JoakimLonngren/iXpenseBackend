using iXpenseBackend.Layers.Services;
using iXpenseBackend.Models.DTO.Receipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iXpenseBackend.Layers.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly ReceiptService _receiptService;

        public ReceiptController(ReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpPost("CreateReceipt")]
        [Authorize]
        public async Task<IActionResult> CreateReceipt([FromBody] CreateReceiptDto createReceiptDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Note to self: Add a extention for this to skip repeat?
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var result = await _receiptService.AddReceiptAsync(createReceiptDto, userId);

            return result.isSuccess
                ? Ok(new { success = true, message = result.message }) 
                : BadRequest(new { success = false, message = result.message });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID was not found." });
            }

            var result = await _receiptService.DeleteReceiptAsync(id, userId);

            return result.isSuccess
                ? Ok(new { success = true, message = result.message })
                : BadRequest(new { success = false, message = result.message });
        }

        [HttpGet("GetUserReceipts")]
        [Authorize]
        public async Task<IActionResult> GetUserReceipts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User is not authenticated." });
            }

            var (isSuccess, message, receipts) = await _receiptService.GetUserReceiptAsync(userId);
            return isSuccess
                ? Ok(new
                {
                    success = true,
                    data = receipts,
                    message
                })
                : NotFound(new
                {
                    success = false,
                    message
                });
        }

        [HttpGet("GetMostPurchasedItem")]
        [Authorize]
        public async Task<IActionResult> GetMostPurchasedItem([FromQuery] DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized(new { success = false, message = "User is not authenticated." });
            }

            var (isSuccess, message, mostBoughtItem) = await _receiptService.GetMostPurchasedItemAsync(userId, startDate, endDate);

            if (!isSuccess)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message, data = mostBoughtItem });

            //return isSuccess
            //    ? Ok(new {  success = true, data = mostBoughtItem, message})
            //    : NotFound(new { success = false, message });
        }

        [HttpGet("GetMostPurchasedCategory")]
        [Authorize]
        public async Task<IActionResult> GetMostPurchasedCategory([FromQuery] DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized(new { success = false, message = "User is not authenticated." });
            }

            var (isSuccess, message, data) = await _receiptService.GetMostPurchasedCategoryAsync(userId, startDate, endDate);

            if (!isSuccess)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message, data });

            //return isSuccess 
            //    ? Ok(new { success = true, message, data })
            //    : NotFound(new { success = false, message });

        }
    }
}
