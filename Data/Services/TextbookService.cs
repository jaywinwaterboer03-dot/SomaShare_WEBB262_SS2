using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaShare.Data.Services
{
    public interface ITextbookService
    {
        Task<List<Textbook>> GetAllAsync(string? search = null, string? author = null, string? isbn = null, string? courseCode = null, string? campus = null, decimal? minPrice = null, decimal? maxPrice = null, string? condition = null, string? sort = null, int page = 1, int pageSize = 10);
        Task<int> CountAsync(string? search = null, string? campus = null, decimal? minPrice = null, decimal? maxPrice = null, string? condition = null);
        Task<List<Textbook>> GetForSellerAsync(string sellerId);
        Task<Textbook?> GetByIdAsync(int id);
        Task AddAsync(Textbook textbook);
        Task UpdateAsync(Textbook textbook);
        Task DeleteAsync(int id);
    }

    public class TextbookService : ITextbookService
    {
        private readonly ApplicationDbContext _context;
        public TextbookService(ApplicationDbContext context) => _context = context;

        public async Task<List<Textbook>> GetAllAsync(string? search = null, string? author = null, string? isbn = null, string? courseCode = null, string? campus = null, decimal? minPrice = null, decimal? maxPrice = null, string? condition = null, string? sort = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Textbooks.Include(t => t.Seller).AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Title.Contains(search) || t.Author.Contains(search) || t.ISBN.Contains(search) || t.CourseCode.Contains(search));
            if (!string.IsNullOrEmpty(author))
                query = query.Where(t => t.Author.Contains(author));
            if (!string.IsNullOrEmpty(isbn))
                query = query.Where(t => t.ISBN.Contains(isbn));
            if (!string.IsNullOrEmpty(campus))
                query = query.Where(t => t.Campus == campus);
            if (minPrice.HasValue)
                query = query.Where(t => t.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(t => t.Price <= maxPrice);
            if (!string.IsNullOrEmpty(condition))
                query = query.Where(t => t.Condition == condition);
            if (sort == "price")
                query = query.OrderBy(t => t.Price);
            else if (sort == "date")
                query = query.OrderByDescending(t => t.CreatedAt);
            else
                query = query.OrderByDescending(t => t.CreatedAt);
            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountAsync(string? search = null, string? campus = null, decimal? minPrice = null, decimal? maxPrice = null, string? condition = null)
        {
            var query = _context.Textbooks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.Title.Contains(search) || t.Author.Contains(search) || t.ISBN.Contains(search) || t.CourseCode.Contains(search));
            if (!string.IsNullOrWhiteSpace(campus))
                query = query.Where(t => t.Campus == campus);
            if (minPrice.HasValue)
                query = query.Where(t => t.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(t => t.Price <= maxPrice);
            if (!string.IsNullOrWhiteSpace(condition))
                query = query.Where(t => t.Condition == condition);
            return await query.CountAsync();
        }

        public async Task<List<Textbook>> GetForSellerAsync(string sellerId) =>
            await _context.Textbooks.Where(t => t.SellerId == sellerId).OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task<Textbook?> GetByIdAsync(int id) => await _context.Textbooks.Include(t => t.Seller).Include(t => t.Offers).ThenInclude(o => o.Buyer).FirstOrDefaultAsync(t => t.Id == id);
        public async Task AddAsync(Textbook textbook) { _context.Textbooks.Add(textbook); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Textbook textbook) { _context.Textbooks.Update(textbook); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var t = await _context.Textbooks.FindAsync(id); if (t != null) { _context.Textbooks.Remove(t); await _context.SaveChangesAsync(); } }
    }
}
