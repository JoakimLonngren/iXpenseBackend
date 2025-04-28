using iXpenseBackend.Data.DbContext;
using iXpenseBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace iXpenseBackend.Layers.Repositories
{
    public class ReceiptRepo
    {
        private readonly Context _context;

        public ReceiptRepo(Context context)
        {
            _context = context;
        }

        //Get a receipt by Id from DB
        public async Task <Receipt?> GetReceiptById(int receiptId)
        {
            return await _context.Receipts
                .AsNoTracking()
                .Include(r => r.Items)
                .ThenInclude(i => i.Category)
                .SingleOrDefaultAsync(r => r.Id == receiptId);
        } 

        //Get all receipts from DB
        public async Task <List<Receipt>> GetAllReceiptsAsync()
        {
            return await _context.Receipts
                .AsNoTracking()
                .Include(i => i.Items)
                .ThenInclude(i => i.Category)
                .ToListAsync();
        }

        //Create a receipt
        public async Task <Receipt> AddReceiptAsync(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();
            return receipt;
        }

        //Gets a receipt created by the user based on userId and receiptId
        public async Task <Receipt?> GetReceiptByIdAndUserIdAsync(int id, string userId)
        {
            return await _context.Receipts
                .AsNoTracking()
                .Include(r => r.Items)
                .FirstOrDefaultAsync( r => r.Id == id && r.UserId == userId);
        }

        //Remove a receipt
        public async Task<bool> DeleteReceiptAsync(Receipt receipt)
        {
            _context.Receipts.Remove(receipt);
            return await _context.SaveChangesAsync() > 0; //Move this logic to service layer?          
        }

        //View receipt made by the user
        public async Task<IEnumerable<Receipt>> GetReceiptByUserIdAsync(string userId)
        {
            return await _context.Receipts
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .Include(r => r.Items)
                .ToListAsync();
        }
    }
}
