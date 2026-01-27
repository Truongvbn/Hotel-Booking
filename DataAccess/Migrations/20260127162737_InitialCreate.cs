using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "amenity",
                columns: table => new
                {
                    amenity_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_amenity", x => x.amenity_id);
                });

            migrationBuilder.CreateTable(
                name: "hotel",
                columns: table => new
                {
                    hotel_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    star_rating = table.Column<decimal>(type: "numeric(2,1)", precision: 2, scale: 1, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric(10,7)", precision: 10, scale: 7, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(10,7)", precision: 10, scale: 7, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotel", x => x.hotel_id);
                });

            migrationBuilder.CreateTable(
                name: "room_type",
                columns: table => new
                {
                    room_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotel_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_type", x => x.room_type_id);
                    table.ForeignKey(
                        name: "FK_room_type_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Customer"),
                    hotel_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_type_id = table.Column<int>(type: "integer", nullable: false),
                    room_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Available"),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.room_id);
                    table.ForeignKey(
                        name: "FK_room_room_type_room_type_id",
                        column: x => x.room_type_id,
                        principalTable: "room_type",
                        principalColumn: "room_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuestCount = table.Column<int>(type: "integer", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    SpecialRequests = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_booking_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "room_amenity",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    amenity_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_amenity", x => new { x.room_id, x.amenity_id });
                    table.ForeignKey(
                        name: "FK_room_amenity_amenity_amenity_id",
                        column: x => x.amenity_id,
                        principalTable: "amenity",
                        principalColumn: "amenity_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_amenity_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payment_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    hotel_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_review_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_hotel_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "hotel",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_review_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "amenity",
                columns: new[] { "amenity_id", "CreatedDate", "Description", "icon", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Free high-speed WiFi", "wifi", "WiFi" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Climate control", "snowflake", "Air Conditioning" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flat-screen TV", "tv", "TV" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "In-room mini bar", "glass-martini", "Mini Bar" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "In-room safe", "lock", "Safe" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Private bathroom", "bath", "Bathroom" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Private balcony", "door-open", "Balcony" },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ocean view", "water", "Sea View" }
                });

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "hotel_id", "address", "city", "country", "created_date", "description", "image_url", "is_active", "latitude", "longitude", "name", "phone", "star_rating", "updated_date", "website" },
                values: new object[,]
                {
                    { 1, "123 Nguyen Hue Street", "Ho Chi Minh", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A luxurious 5-star hotel in the heart of the city with stunning views and world-class amenities.", null, true, null, null, "Grand Luxury Hotel", "028-1234-5678", 5.0m, null, "https://grandluxury.com" },
                    { 2, "456 Tran Phu Street", "Nha Trang", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beautiful beachfront resort with private beach access and spa facilities.", null, true, null, null, "Beach Paradise Resort", "0258-987-6543", 4.5m, null, "https://beachparadise.com" },
                    { 3, "789 Hoang Dieu Street", "Da Lat", "Vietnam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Charming mountain lodge surrounded by pine forests and beautiful gardens.", null, true, null, null, "Mountain View Lodge", "0263-111-2222", 4.0m, null, "https://mountainview.com" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "address", "created_date", "email", "first_name", "hotel_id", "is_active", "last_name", "password", "phone_number", "role", "updated_date" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@hotelbooking.com", "Platform", null, true, "Admin", "Admin@123", "0123456789", "Admin", null },
                    { 2, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "customer@gmail.com", "Nguyen", null, true, "Van A", "Customer@123", "0987654321", "Customer", null }
                });

            migrationBuilder.InsertData(
                table: "room_type",
                columns: new[] { "room_type_id", "base_price", "capacity", "created_date", "description", "hotel_id", "image_url", "name", "updated_date" },
                values: new object[,]
                {
                    { 1, 1500000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comfortable room with essential amenities", 1, null, "Standard Room", null },
                    { 2, 2500000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Spacious room with city view", 1, null, "Deluxe Room", null },
                    { 3, 4500000m, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Luxury suite with living area", 1, null, "Executive Suite", null },
                    { 4, 1800000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Room overlooking tropical gardens", 2, null, "Garden View Room", null },
                    { 5, 2800000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Room with stunning sea views", 2, null, "Ocean View Room", null },
                    { 6, 8000000m, 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Private villa with beach access", 2, null, "Beach Villa", null },
                    { 7, 1200000m, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Warm and cozy room with mountain view", 3, null, "Cozy Room", null },
                    { 8, 2000000m, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Large room for families", 3, null, "Family Room", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "address", "created_date", "email", "first_name", "hotel_id", "is_active", "last_name", "password", "phone_number", "role", "updated_date" },
                values: new object[,]
                {
                    { 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "owner.grandluxury@hotel.com", "Tran", 1, true, "Van B", "Owner@123", "0912345678", "HotelOwner", null },
                    { 4, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "owner.beachparadise@hotel.com", "Le", 2, true, "Thi C", "Owner@123", "0923456789", "HotelOwner", null },
                    { 5, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "owner.mountainview@hotel.com", "Pham", 3, true, "Van D", "Owner@123", "0934567890", "HotelOwner", null }
                });

            migrationBuilder.InsertData(
                table: "room",
                columns: new[] { "room_id", "created_date", "floor", "room_number", "room_type_id", "status", "updated_date" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "101", 1, "Available", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "102", 1, "Available", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "201", 2, "Available", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "202", 2, "Available", null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "301", 3, "Available", null },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "A101", 4, "Available", null },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "A102", 4, "Available", null },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "B201", 5, "Available", null },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "B202", 5, "Available", null },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "V01", 6, "Available", null },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "M101", 7, "Available", null },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "M102", 7, "Available", null },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "M201", 8, "Available", null }
                });

            migrationBuilder.InsertData(
                table: "room_amenity",
                columns: new[] { "amenity_id", "room_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 3 },
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 },
                    { 4, 5 },
                    { 5, 5 },
                    { 6, 5 },
                    { 7, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_check_in_date",
                table: "booking",
                column: "check_in_date");

            migrationBuilder.CreateIndex(
                name: "IX_booking_check_out_date",
                table: "booking",
                column: "check_out_date");

            migrationBuilder.CreateIndex(
                name: "IX_booking_room_id",
                table: "booking",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_user_id",
                table: "booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_booking_id",
                table: "payment",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_booking_id",
                table: "review",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_hotel_id",
                table: "review",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_user_id",
                table: "review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_room_number",
                table: "room",
                column: "room_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_room_type_id",
                table: "room",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_amenity_amenity_id",
                table: "room_amenity",
                column: "amenity_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_type_hotel_id",
                table: "room_type",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_hotel_id",
                table: "users",
                column: "hotel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "room_amenity");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "amenity");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "room_type");

            migrationBuilder.DropTable(
                name: "hotel");
        }
    }
}
