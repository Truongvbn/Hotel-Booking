// using System;
// using Microsoft.EntityFrameworkCore.Migrations;

// #nullable disable

// #pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

// namespace DataAccess.Migrations
// {
//     /// <inheritdoc />
//     public partial class InitialCreate : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.CreateTable(
//                 name: "Amenities",
//                 columns: table => new
//                 {
//                     AmenityId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     Name = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: false),
//                     Description = table.Column<string>(/*type: "nvarchar(255)",*/ maxLength: 255, nullable: true),
//                     Icon = table.Column<string>(/*type: "nvarchar(50)",*/ maxLength: 50, nullable: true),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Amenities", x => x.AmenityId);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Hotels",
//                 columns: table => new
//                 {
//                     HotelId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     Name = table.Column<string>(/*type: "nvarchar(200)",*/ maxLength: 200, nullable: false),
//                     Address = table.Column<string>(/*type: "nvarchar(255)",*/ maxLength: 255, nullable: true),
//                     City = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: true),
//                     Country = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: true),
//                     Description = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     StarRating = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false),
//                     Latitude = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
//                     Longitude = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
//                     Phone = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: true),
//                     Website = table.Column<string>(/*type: "nvarchar(200)",*/ maxLength: 200, nullable: true),
//                     ImageUrl = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true),
//                     IsActive = table.Column<bool>(/*type: "bit",*/ nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Hotels", x => x.HotelId);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Users",
//                 columns: table => new
//                 {
//                     UserId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     Email = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: false),
//                     Password = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: false),
//                     FirstName = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: true),
//                     LastName = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: true),
//                     PhoneNumber = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: true),
//                     Address = table.Column<string>(/*type: "nvarchar(255)",*/ maxLength: 255, nullable: true),
//                     Role = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: false, defaultValue: "Customer"),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true),
//                     IsActive = table.Column<bool>(/*type: "bit",*/ nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Users", x => x.UserId);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "RoomTypes",
//                 columns: table => new
//                 {
//                     RoomTypeId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     HotelId = table.Column<int>(type: "int", nullable: false),
//                     Name = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: false),
//                     Description = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     Capacity = table.Column<int>(type: "int", nullable: false),
//                     BasePrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
//                     ImageUrl = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_RoomTypes", x => x.RoomTypeId);
//                     table.ForeignKey(
//                         name: "FK_RoomTypes_Hotels_HotelId",
//                         column: x => x.HotelId,
//                         principalTable: "Hotels",
//                         principalColumn: "HotelId",
//                         onDelete: ReferentialAction.Cascade);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Rooms",
//                 columns: table => new
//                 {
//                     RoomId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     RoomTypeId = table.Column<int>(type: "int", nullable: false),
//                     RoomNumber = table.Column<string>(/*type: "nvarchar(50)",*/ maxLength: 50, nullable: false),
//                     Floor = table.Column<int>(type: "int", nullable: false),
//                     Status = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: false, defaultValue: "Available"),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Rooms", x => x.RoomId);
//                     table.ForeignKey(
//                         name: "FK_Rooms_RoomTypes_RoomTypeId",
//                         column: x => x.RoomTypeId,
//                         principalTable: "RoomTypes",
//                         principalColumn: "RoomTypeId",
//                         onDelete: ReferentialAction.Cascade);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Bookings",
//                 columns: table => new
//                 {
//                     BookingId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     UserId = table.Column<int>(type: "int", nullable: false),
//                     RoomId = table.Column<int>(type: "int", nullable: false),
//                     CheckInDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     CheckOutDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     GuestCount = table.Column<int>(type: "int", nullable: false),
//                     TotalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
//                     Status = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: false, defaultValue: "Pending"),
//                     SpecialRequests = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Bookings", x => x.BookingId);
//                     table.ForeignKey(
//                         name: "FK_Bookings_Rooms_RoomId",
//                         column: x => x.RoomId,
//                         principalTable: "Rooms",
//                         principalColumn: "RoomId",
//                         onDelete: ReferentialAction.Restrict);
//                     table.ForeignKey(
//                         name: "FK_Bookings_Users_UserId",
//                         column: x => x.UserId,
//                         principalTable: "Users",
//                         principalColumn: "UserId",
//                         onDelete: ReferentialAction.Restrict);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "RoomAmenities",
//                 columns: table => new
//                 {
//                     RoomId = table.Column<int>(type: "int", nullable: false),
//                     AmenityId = table.Column<int>(type: "int", nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_RoomAmenities", x => new { x.RoomId, x.AmenityId });
//                     table.ForeignKey(
//                         name: "FK_RoomAmenities_Amenities_AmenityId",
//                         column: x => x.AmenityId,
//                         principalTable: "Amenities",
//                         principalColumn: "AmenityId",
//                         onDelete: ReferentialAction.Cascade);
//                     table.ForeignKey(
//                         name: "FK_RoomAmenities_Rooms_RoomId",
//                         column: x => x.RoomId,
//                         principalTable: "Rooms",
//                         principalColumn: "RoomId",
//                         onDelete: ReferentialAction.Cascade);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Payments",
//                 columns: table => new
//                 {
//                     PaymentId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     BookingId = table.Column<int>(type: "int", nullable: false),
//                     Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
//                     PaymentMethod = table.Column<string>(/*type: "nvarchar(50)",*/ maxLength: 50, nullable: true),
//                     TransactionId = table.Column<string>(/*type: "nvarchar(100)",*/ maxLength: 100, nullable: true),
//                     Status = table.Column<string>(/*type: "nvarchar(20)",*/ maxLength: 20, nullable: false, defaultValue: "Pending"),
//                     PaymentDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Payments", x => x.PaymentId);
//                     table.ForeignKey(
//                         name: "FK_Payments_Bookings_BookingId",
//                         column: x => x.BookingId,
//                         principalTable: "Bookings",
//                         principalColumn: "BookingId",
//                         onDelete: ReferentialAction.Cascade);
//                 });

//             migrationBuilder.CreateTable(
//                 name: "Reviews",
//                 columns: table => new
//                 {
//                     ReviewId = table.Column<int>(type: "int", nullable: false)
//                         /*.Annotation("SqlServer:Identity", "1, 1")*/,
//                     BookingId = table.Column<int>(type: "int", nullable: false),
//                     UserId = table.Column<int>(type: "int", nullable: false),
//                     HotelId = table.Column<int>(type: "int", nullable: false),
//                     Rating = table.Column<int>(type: "int", nullable: false),
//                     Title = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     Comment = table.Column<string>(/*type: "nvarchar(max)",*/ nullable: true),
//                     IsApproved = table.Column<bool>(/*type: "bit",*/ nullable: false),
//                     CreatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: false),
//                     UpdatedDate = table.Column<DateTime>(/*type: "datetime2",*/ nullable: true)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Reviews", x => x.ReviewId);
//                     table.ForeignKey(
//                         name: "FK_Reviews_Bookings_BookingId",
//                         column: x => x.BookingId,
//                         principalTable: "Bookings",
//                         principalColumn: "BookingId",
//                         onDelete: ReferentialAction.Cascade);
//                     table.ForeignKey(
//                         name: "FK_Reviews_Hotels_HotelId",
//                         column: x => x.HotelId,
//                         principalTable: "Hotels",
//                         principalColumn: "HotelId",
//                         onDelete: ReferentialAction.Restrict);
//                     table.ForeignKey(
//                         name: "FK_Reviews_Users_UserId",
//                         column: x => x.UserId,
//                         principalTable: "Users",
//                         principalColumn: "UserId",
//                         onDelete: ReferentialAction.Restrict);
//                 });

//             migrationBuilder.InsertData(
//                 table: "Amenities",
//                 columns: new[] { "AmenityId", "CreatedDate", "Description", "Icon", "Name" },
//                 values: new object[,]
//                 {
//                     { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Free high-speed WiFi", "wifi", "WiFi" },
//                     { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Climate control", "snowflake", "Air Conditioning" },
//                     { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flat-screen TV", "tv", "TV" },
//                     { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "In-room mini bar", "glass-martini", "Mini Bar" },
//                     { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "In-room safe", "lock", "Safe" },
//                     { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Private bathroom", "bath", "Bathroom" },
//                     { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Private balcony", "door-open", "Balcony" },
//                     { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ocean view", "water", "Sea View" }
//                 });

//             migrationBuilder.InsertData(
//                 table: "Hotels",
//                 columns: new[] { "HotelId", "Address", "City", "Country", "CreatedDate", "Description", "ImageUrl", "IsActive", "Latitude", "Longitude", "Name", "Phone", "StarRating", "UpdatedDate", "Website" },
//                 values: new object[,]
//                 {
//                     { 1, "123 Nguyen Hue Street", "Ho Chi Minh", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A luxurious 5-star hotel in the heart of the city with stunning views and world-class amenities.", null, true, null, null, "Grand Luxury Hotel", "028-1234-5678", 5.0m, null, "https://grandluxury.com" },
//                     { 2, "456 Tran Phu Street", "Nha Trang", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful beachfront resort with private beach access and spa facilities.", null, true, null, null, "Beach Paradise Resort", "0258-987-6543", 4.5m, null, "https://beachparadise.com" },
//                     { 3, "789 Hoang Dieu Street", "Da Lat", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charming mountain lodge surrounded by pine forests and beautiful gardens.", null, true, null, null, "Mountain View Lodge", "0263-111-2222", 4.0m, null, "https://mountainview.com" }
//                 });

//             migrationBuilder.InsertData(
//                 table: "Users",
//                 columns: new[] { "UserId", "Address", "CreatedDate", "Email", "FirstName", "IsActive", "LastName", "Password", "PhoneNumber", "Role", "UpdatedDate" },
//                 values: new object[,]
//                 {
//                     { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@hotel.com", "Admin", true, "System", "Admin@123", "0123456789", "Admin", null },
//                     { 2, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "customer@hotel.com", "Nguyen", true, "Van A", "Customer@123", "0987654321", "Customer", null }
//                 });

//             migrationBuilder.InsertData(
//                 table: "RoomTypes",
//                 columns: new[] { "RoomTypeId", "BasePrice", "Capacity", "CreatedDate", "Description", "HotelId", "ImageUrl", "Name", "UpdatedDate" },
//                 values: new object[,]
//                 {
//                     { 1, 1500000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfortable room with essential amenities", 1, null, "Standard Room", null },
//                     { 2, 2500000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spacious room with city view", 1, null, "Deluxe Room", null },
//                     { 3, 4500000m, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxury suite with living area", 1, null, "Executive Suite", null },
//                     { 4, 1800000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Room overlooking tropical gardens", 2, null, "Garden View Room", null },
//                     { 5, 2800000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Room with stunning sea views", 2, null, "Ocean View Room", null },
//                     { 6, 8000000m, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Private villa with beach access", 2, null, "Beach Villa", null },
//                     { 7, 1200000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warm and cozy room with mountain view", 3, null, "Cozy Room", null },
//                     { 8, 2000000m, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Large room for families", 3, null, "Family Room", null }
//                 });

//             migrationBuilder.InsertData(
//                 table: "Rooms",
//                 columns: new[] { "RoomId", "CreatedDate", "Floor", "RoomNumber", "RoomTypeId", "Status", "UpdatedDate" },
//                 values: new object[,]
//                 {
//                     { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "101", 1, "Available", null },
//                     { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "102", 1, "Available", null },
//                     { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "201", 2, "Available", null },
//                     { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "202", 2, "Available", null },
//                     { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "301", 3, "Available", null },
//                     { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "A101", 4, "Available", null },
//                     { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "A102", 4, "Available", null },
//                     { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "B201", 5, "Available", null },
//                     { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "B202", 5, "Available", null },
//                     { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "V01", 6, "Available", null },
//                     { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "M101", 7, "Available", null },
//                     { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "M102", 7, "Available", null },
//                     { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "M201", 8, "Available", null }
//                 });

//             migrationBuilder.InsertData(
//                 table: "RoomAmenities",
//                 columns: new[] { "AmenityId", "RoomId" },
//                 values: new object[,]
//                 {
//                     { 1, 1 },
//                     { 2, 1 },
//                     { 3, 1 },
//                     { 1, 2 },
//                     { 2, 2 },
//                     { 3, 2 },
//                     { 1, 3 },
//                     { 2, 3 },
//                     { 3, 3 },
//                     { 4, 3 },
//                     { 5, 3 },
//                     { 1, 5 },
//                     { 2, 5 },
//                     { 3, 5 },
//                     { 4, 5 },
//                     { 5, 5 },
//                     { 6, 5 },
//                     { 7, 5 }
//                 });

//             migrationBuilder.CreateIndex(
//                 name: "IX_Bookings_CheckInDate",
//                 table: "Bookings",
//                 column: "CheckInDate");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Bookings_CheckOutDate",
//                 table: "Bookings",
//                 column: "CheckOutDate");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Bookings_RoomId",
//                 table: "Bookings",
//                 column: "RoomId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Bookings_UserId",
//                 table: "Bookings",
//                 column: "UserId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Payments_BookingId",
//                 table: "Payments",
//                 column: "BookingId",
//                 unique: true);

//             migrationBuilder.CreateIndex(
//                 name: "IX_Reviews_BookingId",
//                 table: "Reviews",
//                 column: "BookingId",
//                 unique: true);

//             migrationBuilder.CreateIndex(
//                 name: "IX_Reviews_HotelId",
//                 table: "Reviews",
//                 column: "HotelId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Reviews_UserId",
//                 table: "Reviews",
//                 column: "UserId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_RoomAmenities_AmenityId",
//                 table: "RoomAmenities",
//                 column: "AmenityId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Rooms_RoomNumber",
//                 table: "Rooms",
//                 column: "RoomNumber",
//                 unique: true);

//             migrationBuilder.CreateIndex(
//                 name: "IX_Rooms_RoomTypeId",
//                 table: "Rooms",
//                 column: "RoomTypeId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_RoomTypes_HotelId",
//                 table: "RoomTypes",
//                 column: "HotelId");

//             migrationBuilder.CreateIndex(
//                 name: "IX_Users_Email",
//                 table: "Users",
//                 column: "Email",
//                 unique: true);
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropTable(
//                 name: "Payments");

//             migrationBuilder.DropTable(
//                 name: "Reviews");

//             migrationBuilder.DropTable(
//                 name: "RoomAmenities");

//             migrationBuilder.DropTable(
//                 name: "Bookings");

//             migrationBuilder.DropTable(
//                 name: "Amenities");

//             migrationBuilder.DropTable(
//                 name: "Rooms");

//             migrationBuilder.DropTable(
//                 name: "Users");

//             migrationBuilder.DropTable(
//                 name: "RoomTypes");

//             migrationBuilder.DropTable(
//                 name: "Hotels");
//         }
//     }
// }
