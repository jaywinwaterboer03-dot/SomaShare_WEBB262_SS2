# SomaShare

SomaShare is a Blazor-based university textbook exchange platform for buying, selling, requesting, and reviewing textbooks in a trusted campus community.

## Technologies

- ASP.NET Core Blazor Server
- .NET 10 SDK, meeting the assignment requirement of .NET 8+
- C#
- ASP.NET Core Identity with roles
- Entity Framework Core Code-First
- SQL Server LocalDB

## Main Features

- Student registration and login with university email validation
- Buyer, Seller, and Admin roles
- Textbook listings with CRUD operations
- Wanted ads
- Offers and seller accept/reject workflow
- Transaction creation when an offer is accepted
- Cash on Meetup payment option
- Transaction completion
- Ratings, reviews, and trust score
- Search by title, author, ISBN, and course code
- Filters for campus, condition, and price range
- Sorting and pagination
- User dashboard for listings, offers, transactions, reviews, and trust score
- Admin dashboard for listing moderation and account suspension
- English/isiZulu profile language preference

## Database

The application uses the `SomaShareDB` SQL Server LocalDB database. The connection string is stored in `appsettings.json` as `DefaultConnection`.

Migrations are stored in `Data/Migrations`, including:

- Identity schema
- SomaShare domain schema for users, textbooks, wanted ads, offers, transactions, reviews, categories, and the textbook/category many-to-many relationship

## Seeded Test Accounts

| Role | Email | Password |
| --- | --- | --- |
| Admin | admin@university.ac.za | Admin123! |
| Buyer | buyer@student.ac.za | Buyer123! |
| Seller | seller@student.ac.za | Seller123! |

## Run Locally

```powershell
dotnet build
dotnet run --urls http://localhost:5087
```

Open `http://localhost:5087` in the browser.


