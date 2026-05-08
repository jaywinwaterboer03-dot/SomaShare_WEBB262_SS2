using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data.Services
{
    public interface IWantedAdService
    {
        Task<List<WantedAd>> GetAllAsync();
        Task<List<WantedAd>> GetForBuyerAsync(string buyerId);
        Task<WantedAd?> GetByIdAsync(int id);
        Task AddAsync(WantedAd wantedAd);
        Task UpdateAsync(WantedAd wantedAd);
        Task DeleteAsync(int id);
    }

    public class WantedAdService(ApplicationDbContext context) : IWantedAdService
    {
        public async Task<List<WantedAd>> GetAllAsync() =>
            await context.WantedAds.Include(ad => ad.Buyer).OrderByDescending(ad => ad.CreatedAt).ToListAsync();

        public async Task<List<WantedAd>> GetForBuyerAsync(string buyerId) =>
            await context.WantedAds.Where(ad => ad.BuyerId == buyerId).OrderByDescending(ad => ad.CreatedAt).ToListAsync();

        public async Task<WantedAd?> GetByIdAsync(int id) => await context.WantedAds.FindAsync(id);

        public async Task AddAsync(WantedAd wantedAd)
        {
            context.WantedAds.Add(wantedAd);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WantedAd wantedAd)
        {
            context.WantedAds.Update(wantedAd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wantedAd = await context.WantedAds.FindAsync(id);
            if (wantedAd is null)
            {
                return;
            }

            context.WantedAds.Remove(wantedAd);
            await context.SaveChangesAsync();
        }
    }
}
