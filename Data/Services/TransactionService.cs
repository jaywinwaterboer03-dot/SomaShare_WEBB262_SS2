using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaShare.Data.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsForUserAsync(string userId);
        Task<Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        public TransactionService(ApplicationDbContext context) => _context = context;

        public async Task<List<Transaction>> GetTransactionsForUserAsync(string userId) => await _context.Transactions.Include(t => t.Textbook).Include(t => t.Buyer).Include(t => t.Seller).Where(t => t.BuyerId == userId || t.SellerId == userId).OrderByDescending(t => t.TransactionDate).ToListAsync();
        public async Task<Transaction?> GetByIdAsync(int id) => await _context.Transactions.Include(t => t.Textbook).Include(t => t.Buyer).Include(t => t.Seller).Include(t => t.Reviews).FirstOrDefaultAsync(t => t.Id == id);
        public async Task AddAsync(Transaction transaction) { _context.Transactions.Add(transaction); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Transaction transaction) { _context.Transactions.Update(transaction); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var t = await _context.Transactions.FindAsync(id); if (t != null) { _context.Transactions.Remove(t); await _context.SaveChangesAsync(); } }
    }
}
