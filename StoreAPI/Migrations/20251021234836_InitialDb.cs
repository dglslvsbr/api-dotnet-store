using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UsedLimit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, defaultValue: 0.0m),
                    MaxLimit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCard_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    Installments = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRole",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRole", x => new { x.ClientId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ClientRole_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Hardware" },
                    { 2, "PlayStation" },
                    { 3, "Smartphone" },
                    { 4, "Games" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.", "assets/images/GraphicCard4070Ti.png", "RTX 4070 Ti", 7500m, 0 },
                    { 2, 1, "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.", "assets/images/GraphicCard5090.png", "RTX 5090", 20999m, 0 },
                    { 3, 2, "The latest console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.", "assets/images/PlayStation5.png", "PlayStation 5", 3299m, 0 },
                    { 4, 2, "Console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.", "assets/images/PlayStation4.png", "PlayStation 4", 2299m, 0 },
                    { 5, 3, "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.", "assets/images/XiaomiRedmiNote13.png", "Redmi Note 13", 3599m, 0 },
                    { 6, 3, "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.", "assets/images/Iphone14.png", "Iphone 14", 6500m, 0 },
                    { 7, 1, "Can deliver fast performance of over 100 FPS in the world's most popular games, discrete graphics required", "assets/images/Processor5500.png", "AMD Ryzen 5 5500", 590m, 0 },
                    { 8, 1, "The best processor for gamers meets the best processor for creators, with 16 cores and 32 processing lines", "assets/images/Processor5950X.png", "AMD Ryzen 9 5950X", 3999m, 0 },
                    { 9, 1, "Can deliver ultra-fast 100 FPS performance in the world's most popular games, discrete graphics card required", "assets/images/Processor5700X.png", "AMD Ryzen 7 5700X", 1089m, 0 },
                    { 10, 4, "Winner of over 300 Game of the Year awards, remastered for the PS5 console.", "assets/images/TheLastOfUs2.png", "The Last of Us", 220m, 0 },
                    { 11, 4, "Embark on an epic and heartbreaking journey where Kratos and Atreus struggle between the desire to stay together or separate.", "assets/images/GowRagnarok.png", "God of War", 220m, 0 },
                    { 12, 2, "Sony's latest limited edition console, more efficient than previous generations, offering maximum performance and excellent graphics.", "assets/images/PlayStation5Edition.png", "PlayStation 5 Gold", 4899m, 0 },
                    { 13, 2, "All the Classics You Love, Plus 93 Thousand Titles!", "assets/images/ConsoleRetroPSX.png", "Console Retrô PSX", 2399m, 0 },
                    { 14, 4, "The Golden Order has been broken. Rise, Tarnished, and be guided by grace to wield the power of the Pristine Ring and become a Pristine Lord in the Lands Between.", "assets/images/EldenRing.png", "Elden Ring", 220m, 0 },
                    { 15, 2, "Your PS5 in the palm of your hand with the PlayStation Portal Remote Player", "assets/images/PlayStationPortatil.png", "PlayStation Portátil", 4100m, 0 },
                    { 16, 4, "Resident Evil 4 is a remake of the original 2005 game. With revamped graphics, updated gameplay, and a reimagined storyline, while preserving the essence of the original game.Resident Evil 4 is a remake of the original 2005 game. With revamped graphics, updated gameplay, and a reimagined storyline, while preserving the essence of the original game.", "assets/images/ResidentEvil4.png", "Resident Evil 4", 220m, 0 },
                    { 17, 4, "Experience the rise of Miles Morales as the hero who masters the new, amazing, and explosive powers to become Spider-Man himself", "assets/images/SpiderManMilesMorales.png", "Miles Morales", 220m, 0 },
                    { 18, 3, "50 MP main camera with AI and Automatic Night Vision", "assets/images/MotorolaG15.png", "Motorola G15", 889m, 0 },
                    { 19, 3, "Multiple actions in a single voice command", "assets/images/SamsungGalaxyS25.png", "Samsung Galaxy S25", 3999m, 0 },
                    { 20, 3, "Processing and screen to compete and win. New Mediatek Dimensity 8400-Ultra processor and 1.5 curved AMOLED screen with 120Hz, your new high-performance mobile gaming setup. Express your creativity and share your stories with quality.", "assets/images/XiaomiPocoX17.png", "Xiaomi Poco X17", 2199m, 0 },
                    { 21, 1, "Equipped with the powerful 13th-generation Intel Core i7-13620H processor and the NVIDIA GeForce RTX 3050 graphics card, this laptop delivers the performance you need to tackle any challenge.", "assets/images/NotebookAcerNitro.png", "Notebook", 4599m, 0 },
                    { 22, 1, "Wireless gaming mouse Attack Shark X11 white – lightweight, precision, and advanced connectivityThe Attack Shark X11 white is the ideal choice for those seeking professional performance combined with a lightweight and modern design.", "assets/images/MouseBranco.png", "Mouse Branco", 189m, 0 },
                    { 23, 1, "If you are looking for an immersive experience for various types of games, such as strategy, FPS, and racing, this monitor is perfect for you.", "assets/images/MonitorUltraGear.png", "Monitor", 1459m, 0 },
                    { 24, 1, "Experience a new dimension of audio with the Redragon Zeus X gaming headset. Its 53mm drivers deliver rich and powerful sound, with deep bass and crystal-clear highs. With 7.1 virtual surround sound, you will hear every detail of the game, from enemy footsteps to the most intense explosions.", "assets/images/HeadsetRedragon.png", "HeadsetRedragon", 199m, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ClientId",
                table: "Address",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_CPF",
                table: "Client",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_Email",
                table: "Client",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_Phone",
                table: "Client",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientRole_RoleId",
                table: "ClientRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_ClientId",
                table: "CreditCard",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_Number",
                table: "CreditCard",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientId",
                table: "Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ClientRole");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
