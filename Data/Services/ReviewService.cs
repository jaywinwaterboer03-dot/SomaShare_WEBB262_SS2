using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaShare.Data.Services
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsForUserAsync(string userId);
        Task<double> GetTrustScoreAsync(string userId);
        Task<Review?> GetByIdAsync(int id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        public ReviewService(ApplicationDbContext context) => _context = context;

        public async Task<List<Review>> GetReviewsForUserAsync(string userId) => await _context.Reviews.Include(r => r.Reviewer).Include(r => r.ReviewedUser).Include(r => r.Transaction).ThenInclude(t => t.Textbook).Where(r => r.ReviewerId == userId || r.ReviewedUserId == userId).OrderByDescending(r => r.CreatedAt).ToListAsync();
        public async Task<double> GetTrustScoreAsync(string userId)
        {
            var ratings = await _context.Reviews.Where(r => r.ReviewedUserId == userId).Select(r => r.Rating).ToListAsync();
            return ratings.Count == 0 ? 0 : Math.Round(ratings.Average(), 1);
        }
        public async Task<Review?> GetByIdAsync(int id) => await _context.Reviews.FindAsync(id);
        public async Task AddAsync(Review review) { _context.Reviews.Add(review); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Review review) { _context.Reviews.Update(review); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var r = await _context.Reviews.FindAsync(id); if (r != null) { _context.Reviews.Remove(r); await _context.SaveChangesAsync(); } }
    }
}
