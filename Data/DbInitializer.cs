using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SomaShare.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            // Seed roles
            string[] roles = { "Admin", "Buyer", "Seller" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed users
            if (userManager.Users.Any()) return; // Already seeded

            var admin = new ApplicationUser { UserName = "admin@university.ac.za", Email = "admin@university.ac.za", EmailConfirmed = true, FullName = "Admin User", Campus = "Main" };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");

            var buyer = new ApplicationUser { UserName = "buyer@student.ac.za", Email = "buyer@student.ac.za", EmailConfirmed = true, FullName = "Buyer Student", Campus = "East" };
            await userManager.CreateAsync(buyer, "Buyer123!");
            await userManager.AddToRoleAsync(buyer, "Buyer");

            var seller = new ApplicationUser { UserName = "seller@student.ac.za", Email = "seller@student.ac.za", EmailConfirmed = true, FullName = "Seller Student", Campus = "West" };
            await userManager.CreateAsync(seller, "Seller123!");
            await userManager.AddToRoleAsync(seller, "Seller");

            // Seed sample textbooks
            var textbook = new Textbook
            {
                Title = "Introduction to Algorithms",
                Author = "Cormen et al.",
                ISBN = "9780262033848",
                CourseCode = "CSC201",
                Condition = "Good",
                Price = 500,
                Campus = "Main",
                SellerId = seller.Id,
                ImagePath = "images/sample1.jpg"
            };
            context.Textbooks.Add(textbook);
            await context.SaveChangesAsync();

            var categories = new[]
            {
                new Category { Name = "Computer Science" },
                new Category { Name = "Business" },
                new Category { Name = "African Scholarship" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
            context.TextbookCategories.Add(new TextbookCategory { TextbookId = textbook.Id, CategoryId = categories[0].Id });
            await context.SaveChangesAsync();

            // Seed wanted ad
            var wantedAd = new WantedAd
            {
                Title = "Database Systems",
                Author = "Elmasri & Navathe",
                ISBN = "9780133970777",
                CourseCode = "CS202",
                Description = "Looking for latest edition.",
                Campus = "East",
                BuyerId = buyer.Id
            };
            context.WantedAds.Add(wantedAd);
            await context.SaveChangesAsync();

            // Seed offer
            var offer = new Offer
            {
                TextbookId = textbook.Id,
                BuyerId = buyer.Id,
                OfferPrice = 450,
                Status = OfferStatus.Accepted
            };
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            // Seed transaction
            var transaction = new Transaction
            {
                TextbookId = textbook.Id,
                OfferId = offer.Id,
                BuyerId = buyer.Id,
                SellerId = seller.Id,
                IsComplete = true,
                PaymentMethod = PaymentMethod.CashOnMeetup
            };
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            // Seed review
            var review = new Review
            {
                TransactionId = transaction.Id,
                ReviewerId = buyer.Id,
                ReviewedUserId = seller.Id,
                Rating = 5,
                Comment = "Great seller, smooth transaction!"
            };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();
        }
    }
}
