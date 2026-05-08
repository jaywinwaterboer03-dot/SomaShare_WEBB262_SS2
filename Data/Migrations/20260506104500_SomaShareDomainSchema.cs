using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomaShare.Migrations
{
    public partial class SomaShareDomainSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Campus",
                table: "AspNetUsers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Main Campus");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "English");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Textbooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Campus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Textbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Textbooks_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WantedAds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Campus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WantedAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WantedAds_AspNetUsers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextbookId = table.Column<int>(type: "int", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_AspNetUsers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_Textbooks_TextbookId",
                        column: x => x.TextbookId,
                        principalTable: "Textbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextbookCategories",
                columns: table => new
                {
                    TextbookId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextbookCategories", x => new { x.TextbookId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_TextbookCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextbookCategories_Textbooks_TextbookId",
                        column: x => x.TextbookId,
                        principalTable: "Textbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextbookId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey("FK_Transactions_AspNetUsers_BuyerId", x => x.BuyerId, "AspNetUsers", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Transactions_AspNetUsers_SellerId", x => x.SellerId, "AspNetUsers", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Transactions_Offers_OfferId", x => x.OfferId, "Offers", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Transactions_Textbooks_TextbookId", x => x.TextbookId, "Textbooks", "Id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReviewedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey("FK_Reviews_AspNetUsers_ReviewedUserId", x => x.ReviewedUserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Reviews_AspNetUsers_ReviewerId", x => x.ReviewerId, "AspNetUsers", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Reviews_Transactions_TransactionId", x => x.TransactionId, "Transactions", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_AspNetUsers_Email", "AspNetUsers", "Email", unique: true, filter: "[Email] IS NOT NULL");
            migrationBuilder.CreateIndex("IX_Textbooks_SellerId", "Textbooks", "SellerId");
            migrationBuilder.CreateIndex("IX_Textbooks_Title_Author_ISBN_CourseCode", "Textbooks", new[] { "Title", "Author", "ISBN", "CourseCode" });
            migrationBuilder.CreateIndex("IX_WantedAds_BuyerId", "WantedAds", "BuyerId");
            migrationBuilder.CreateIndex("IX_Offers_BuyerId", "Offers", "BuyerId");
            migrationBuilder.CreateIndex("IX_Offers_TextbookId", "Offers", "TextbookId");
            migrationBuilder.CreateIndex("IX_TextbookCategories_CategoryId", "TextbookCategories", "CategoryId");
            migrationBuilder.CreateIndex("IX_Transactions_BuyerId", "Transactions", "BuyerId");
            migrationBuilder.CreateIndex("IX_Transactions_OfferId", "Transactions", "OfferId", unique: true);
            migrationBuilder.CreateIndex("IX_Transactions_SellerId", "Transactions", "SellerId");
            migrationBuilder.CreateIndex("IX_Transactions_TextbookId", "Transactions", "TextbookId");
            migrationBuilder.CreateIndex("IX_Reviews_ReviewedUserId", "Reviews", "ReviewedUserId");
            migrationBuilder.CreateIndex("IX_Reviews_ReviewerId", "Reviews", "ReviewerId");
            migrationBuilder.CreateIndex("IX_Reviews_TransactionId", "Reviews", "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Reviews");
            migrationBuilder.DropTable("TextbookCategories");
            migrationBuilder.DropTable("WantedAds");
            migrationBuilder.DropTable("Transactions");
            migrationBuilder.DropTable("Categories");
            migrationBuilder.DropTable("Offers");
            migrationBuilder.DropTable("Textbooks");
            migrationBuilder.DropIndex("IX_AspNetUsers_Email", "AspNetUsers");
            migrationBuilder.DropColumn("Campus", "AspNetUsers");
            migrationBuilder.DropColumn("FullName", "AspNetUsers");
            migrationBuilder.DropColumn("PreferredLanguage", "AspNetUsers");
        }
    }
}
