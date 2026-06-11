# SomaShare

SomaShare is a Blazor-based university textbook exchange platform for buying, selling, requesting, and reviewing textbooks in a trusted campus community.

## Technologies

- ASP.NET Core Blazor Server (.NET 10)
- C#
- ASP.NET Core Identity with roles
- Entity Framework Core Code-First
- SQL Server LocalDB
- SignalR (Real-time notifications)
- Bootstrap (Responsive UI)

## Core Features

- Student registration and login with university email validation
- Buyer, Seller, and Admin roles
- Textbook listings with CRUD operations
- Wanted ads for textbook requests
- Offers and seller accept/reject workflow
- Transaction creation and completion
- Cash on Meetup payment option
- Ratings, reviews, and trust score calculation
- Advanced search (title, author, ISBN, course code)
- Dynamic filtering (campus, condition, price range)
- Sorting and pagination
- User dashboard for personal activity
- Favorites/Wishlist system
- English/isiZulu language preference

## 🆕 Innovation Features (SS3)

### 1. Real-Time Notifications System
- In-app notifications for offers, transactions, reviews
- SignalR-based real-time delivery
- Notification dashboard at `/notifications`
- Mark as read functionality
- Automatic cleanup of old notifications

### 2. User Analytics Dashboard
- Personal sales performance metrics
- Revenue tracking and trends
- Top selling categories analysis
- Monthly sales trends
- Platform-wide analytics
- Access at `/analytics`

### 3. Advanced Admin Reporting
- Comprehensive platform statistics
- Detailed user, transaction, and review reports
- CSV export functionality
- Summary cards with key metrics
- Admin reporting dashboard at `/admin/reports`

## Database

The application uses the `SomaShareDB` SQL Server LocalDB database. Connection string stored in `appsettings.json`.

Migrations include:
- Identity schema
- SomaShare domain entities (users, textbooks, offers, transactions, reviews, categories)
- Notification entity for real-time notifications
- Proper relationships and constraints
- Database indexes for performance

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

## Key Pages

**Public Pages:**
- `/` - Home page
- `/Account/Register` - User registration
- `/Account/Login` - User login

**Authenticated User Pages:**
- `/listings` - Browse textbooks
- `/wanted` - Wanted ads
- `/dashboard` - Personal dashboard
- `/notifications` - Notifications (NEW)
- `/analytics` - Sales analytics (NEW)
- `/Account/Manage` - Account settings

**Admin Pages:**
- `/admin` - Admin dashboard
- `/admin/reports` - Advanced reports (NEW)

## Architecture

**Layered Architecture:**
- **Presentation Layer**: Razor components (`Components/`)
- **Service Layer**: Business logic services (`Data/Services/`)
- **Data Layer**: Entity Framework Core with repositories
- **Real-Time Layer**: SignalR hub (`Hubs/`)

**Design Patterns:**
- Repository pattern
- Service pattern
- Dependency Injection
- Async/await for all I/O operations
- SOLID principles throughout

## Documentation

- `INNOVATION_FEATURES.md` - Detailed feature documentation
- `IMPLEMENTATION_SUMMARY.md` - Implementation overview
- `DEPLOYMENT_GUIDE.md` - Deployment and integration guide
- `SS3_INNOVATION_SUMMARY.md` - SS3 submission overview

## Deployment

The application is ready for Azure App Service deployment:
1. Update database connection string
2. Run migrations: `Update-Database`
3. Use Visual Studio Publish feature
4. Deploy to Azure App Service

## Future Enhancements

- Email notifications
- Advanced analytics charting
- Real-time dashboard updates
- Scheduled reports
- Wishlist notifications
- Advanced admin filtering
- QR code verification system

## SS3 Improvements

- Improved user profile handling
- Added Bio and Preferred Meetup Campus fields
- Added Afrikaans language option
- Added Wanted Ad edit functionality
- Improved validation and user messages
- Cleaned up navigation and removed duplicate textbook menu item
