using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaShare.Data.Services
{
    public interface IOfferService
    {
        Task<List<Offer>> GetOffersForTextbookAsync(int textbookId);
        Task<List<Offer>> GetOffersForUserAsync(string userId);
        Task<Offer?> GetByIdAsync(int id);
        Task AddAsync(Offer offer);
        Task<Transaction> AcceptOfferAsync(int offerId, string sellerId);
        Task RejectOfferAsync(int offerId, string sellerId);
        Task UpdateAsync(Offer offer);
        Task DeleteAsync(int id);
    }

    public class OfferService : IOfferService
    {
        private readonly ApplicationDbContext _context;
        public OfferService(ApplicationDbContext context) => _context = context;

        public async Task<List<Offer>> GetOffersForTextbookAsync(int textbookId) => await _context.Offers.Include(o => o.Buyer).Where(o => o.TextbookId == textbookId).ToListAsync();
        public async Task<List<Offer>> GetOffersForUserAsync(string userId) => await _context.Offers.Include(o => o.Textbook).ThenInclude(t => t.Seller).Where(o => o.BuyerId == userId || o.Textbook.SellerId == userId).OrderByDescending(o => o.OfferedAt).ToListAsync();
        public async Task<Offer?> GetByIdAsync(int id) => await _context.Offers.Include(o => o.Textbook).FirstOrDefaultAsync(o => o.Id == id);
        public async Task AddAsync(Offer offer) { _context.Offers.Add(offer); await _context.SaveChangesAsync(); }
        public async Task<Transaction> AcceptOfferAsync(int offerId, string sellerId)
        {
            var offer = await _context.Offers.Include(o => o.Textbook).FirstOrDefaultAsync(o => o.Id == offerId)
                ?? throw new InvalidOperationException("Offer not found.");

            if (offer.Textbook.SellerId != sellerId)
                throw new InvalidOperationException("Only the seller can accept this offer.");

            if (await _context.Transactions.AnyAsync(t => t.TextbookId == offer.TextbookId))
                throw new InvalidOperationException("This listing already has an accepted offer.");

            offer.Status = OfferStatus.Accepted;
            var otherOffers = await _context.Offers.Where(o => o.TextbookId == offer.TextbookId && o.Id != offer.Id).ToListAsync();
            foreach (var otherOffer in otherOffers)
            {
                otherOffer.Status = OfferStatus.Rejected;
            }

            var transaction = new Transaction
            {
                TextbookId = offer.TextbookId,
                OfferId = offer.Id,
                BuyerId = offer.BuyerId,
                SellerId = sellerId,
                PaymentMethod = PaymentMethod.CashOnMeetup
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task RejectOfferAsync(int offerId, string sellerId)
        {
            var offer = await _context.Offers.Include(o => o.Textbook).FirstOrDefaultAsync(o => o.Id == offerId)
                ?? throw new InvalidOperationException("Offer not found.");
            if (offer.Textbook.SellerId != sellerId)
                throw new InvalidOperationException("Only the seller can reject this offer.");
            offer.Status = OfferStatus.Rejected;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Offer offer) { _context.Offers.Update(offer); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var o = await _context.Offers.FindAsync(id); if (o != null) { _context.Offers.Remove(o); await _context.SaveChangesAsync(); } }
    }
}
