using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data.Services
{
    public interface IFavouriteService
    {
        Task<List<Favourite>> GetUserFavouritesAsync(string userId);
        Task<bool> IsFavouritedAsync(string userId, int textbookId);
        Task AddAsync(string userId, int textbookId);
        Task RemoveAsync(string userId, int textbookId);
        Task RemoveFromFavouritesAsync(string userId, int textbookId); // added - naming match
    }

    public class FavouriteService(ApplicationDbContext context) : IFavouriteService
    {
        public async Task<List<Favourite>> GetUserFavouritesAsync(string userId) =>
            await context.Favourites
                .Include(f => f.Textbook)
                .ThenInclude(t => t.Seller)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();

        public async Task<bool> IsFavouritedAsync(string userId, int textbookId) =>
            await context.Favourites.AnyAsync(f => f.UserId == userId && f.TextbookId == textbookId);

        public async Task AddAsync(string userId, int textbookId)
        {
            if (!await IsFavouritedAsync(userId, textbookId))
            {
                var favourite = new Favourite { UserId = userId, TextbookId = textbookId };
                context.Favourites.Add(favourite);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(string userId, int textbookId)
        {
            var favourite = await context.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TextbookId == textbookId);

            if (favourite != null)
            {
                context.Favourites.Remove(favourite);
                await context.SaveChangesAsync();
            }
        }

        // added wrapper to match call-site name
        public async Task RemoveFromFavouritesAsync(string userId, int textbookId)
        {
            await RemoveAsync(userId, textbookId);
        }
    }
}
