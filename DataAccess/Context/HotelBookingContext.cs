using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace DataAccess.Context;

public class HotelBookingContext : DbContext
{
    public HotelBookingContext(DbContextOptions<HotelBookingContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RoomAmenity> RoomAmenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(100).IsRequired();
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(255);
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20).HasDefaultValue("Customer");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            
            // HotelOwner relationship
            entity.HasOne(e => e.Hotel)
                  .WithMany()
                  .HasForeignKey(e => e.HotelId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Hotel configuration
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("hotel");
            entity.HasKey(e => e.HotelId);
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(255);
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
            entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
            entity.Property(e => e.StarRating).HasColumnName("star_rating").HasPrecision(2, 1);
            entity.Property(e => e.Latitude).HasColumnName("latitude").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("longitude").HasPrecision(10, 7);
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.Website).HasColumnName("website").HasMaxLength(200);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
        });

        // RoomType configuration
        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.ToTable("room_type");
            entity.HasKey(e => e.RoomTypeId);
            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.BasePrice).HasColumnName("base_price").HasPrecision(10, 2);
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(e => e.Hotel)
                  .WithMany(h => h.RoomTypes)
                  .HasForeignKey(e => e.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Room configuration
        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("room");
            entity.HasKey(e => e.RoomId);
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.HasIndex(e => e.RoomNumber).IsUnique();
            entity.Property(e => e.RoomNumber).HasColumnName("room_number").HasMaxLength(50);
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("Available");
            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(e => e.RoomType)
                  .WithMany(rt => rt.Rooms)
                  .HasForeignKey(e => e.RoomTypeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("booking");
            entity.HasKey(e => e.BookingId);
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasPrecision(10, 2);
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Bookings)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Room)
                  .WithMany(r => r.Bookings)
                  .HasForeignKey(e => e.RoomId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            // Indexes for performance
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CheckInDate);
            entity.HasIndex(e => e.CheckOutDate);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payment");
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.HasIndex(e => e.BookingId).IsUnique();
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(10, 2);
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("Pending");

            entity.HasOne(e => e.Booking)
                  .WithOne(b => b.Payment)
                  .HasForeignKey<Payment>(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");
            entity.HasKey(e => e.ReviewId);
            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");

            entity.HasOne(e => e.Booking)
                  .WithOne(b => b.Review)
                  .HasForeignKey<Review>(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Hotel)
                  .WithMany(h => h.Reviews)
                  .HasForeignKey(e => e.HotelId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.HotelId);
        });

        // Amenity configuration
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.ToTable("amenity");
            entity.HasKey(e => e.AmenityId);
            entity.Property(e => e.AmenityId).HasColumnName("amenity_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(50);
        });

        // RoomAmenity (Many-to-Many) configuration
        modelBuilder.Entity<RoomAmenity>(entity =>
        {
            entity.ToTable("room_amenity");
            entity.HasKey(e => new { e.RoomId, e.AmenityId });
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.AmenityId).HasColumnName("amenity_id");

            entity.HasOne(e => e.Room)
                  .WithMany(r => r.RoomAmenities)
                  .HasForeignKey(e => e.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Amenity)
                  .WithMany(a => a.RoomAmenities)
                  .HasForeignKey(e => e.AmenityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Use fixed date for seed data (EF Core doesn't support DateTime.Now in migrations)
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Email = "admin@hotelbooking.com",
                Password = "Admin@123", // In production, this should be hashed
                FirstName = "Platform",
                LastName = "Admin",
                PhoneNumber = "0123456789",
                Role = "Admin",
                HotelId = null, // Platform admin, not tied to any hotel
                IsActive = true,
                CreatedDate = seedDate
            },
            new User
            {
                UserId = 2,
                Email = "customer@gmail.com",
                Password = "Customer@123",
                FirstName = "Nguyen",
                LastName = "Van A",
                PhoneNumber = "0987654321",
                Role = "Customer",
                HotelId = null,
                IsActive = true,
                CreatedDate = seedDate
            },
            new User
            {
                UserId = 3,
                Email = "owner.grandluxury@hotel.com",
                Password = "Owner@123",
                FirstName = "Tran",
                LastName = "Van B",
                PhoneNumber = "0912345678",
                Role = "HotelOwner",
                HotelId = 1, // Owns Grand Luxury Hotel
                IsActive = true,
                CreatedDate = seedDate
            },
            new User
            {
                UserId = 4,
                Email = "owner.beachparadise@hotel.com",
                Password = "Owner@123",
                FirstName = "Le",
                LastName = "Thi C",
                PhoneNumber = "0923456789",
                Role = "HotelOwner",
                HotelId = 2, // Owns Beach Paradise Resort
                IsActive = true,
                CreatedDate = seedDate
            },
            new User
            {
                UserId = 5,
                Email = "owner.mountainview@hotel.com",
                Password = "Owner@123",
                FirstName = "Pham",
                LastName = "Van D",
                PhoneNumber = "0934567890",
                Role = "HotelOwner",
                HotelId = 3, // Owns Mountain View Lodge
                IsActive = true,
                CreatedDate = seedDate
            }
        );

        // Seed Amenities
        modelBuilder.Entity<Amenity>().HasData(
            new Amenity { AmenityId = 1, Name = "WiFi", Description = "Free high-speed WiFi", Icon = "wifi", CreatedDate = seedDate },
            new Amenity { AmenityId = 2, Name = "Air Conditioning", Description = "Climate control", Icon = "snowflake", CreatedDate = seedDate },
            new Amenity { AmenityId = 3, Name = "TV", Description = "Flat-screen TV", Icon = "tv", CreatedDate = seedDate },
            new Amenity { AmenityId = 4, Name = "Mini Bar", Description = "In-room mini bar", Icon = "glass-martini", CreatedDate = seedDate },
            new Amenity { AmenityId = 5, Name = "Safe", Description = "In-room safe", Icon = "lock", CreatedDate = seedDate },
            new Amenity { AmenityId = 6, Name = "Bathroom", Description = "Private bathroom", Icon = "bath", CreatedDate = seedDate },
            new Amenity { AmenityId = 7, Name = "Balcony", Description = "Private balcony", Icon = "door-open", CreatedDate = seedDate },
            new Amenity { AmenityId = 8, Name = "Sea View", Description = "Ocean view", Icon = "water", CreatedDate = seedDate }
        );

        // Seed Hotels
        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                HotelId = 1,
                Name = "Grand Luxury Hotel",
                Address = "123 Nguyen Hue Street",
                City = "Ho Chi Minh",
                Country = "Vietnam",
                Description = "A luxurious 5-star hotel in the heart of the city with stunning views and world-class amenities.",
                StarRating = 5.0m,
                Phone = "028-1234-5678",
                Website = "https://grandluxury.com",
                IsActive = true,
                CreatedDate = seedDate
            },
            new Hotel
            {
                HotelId = 2,
                Name = "Beach Paradise Resort",
                Address = "456 Tran Phu Street",
                City = "Nha Trang",
                Country = "Vietnam",
                Description = "Beautiful beachfront resort with private beach access and spa facilities.",
                StarRating = 4.5m,
                Phone = "0258-987-6543",
                Website = "https://beachparadise.com",
                IsActive = true,
                CreatedDate = seedDate
            },
            new Hotel
            {
                HotelId = 3,
                Name = "Mountain View Lodge",
                Address = "789 Hoang Dieu Street",
                City = "Da Lat",
                Country = "Vietnam",
                Description = "Charming mountain lodge surrounded by pine forests and beautiful gardens.",
                StarRating = 4.0m,
                Phone = "0263-111-2222",
                Website = "https://mountainview.com",
                IsActive = true,
                CreatedDate = seedDate
            }
        );

        // Seed Room Types
        modelBuilder.Entity<RoomType>().HasData(
            // Hotel 1 - Grand Luxury Hotel
            new RoomType { RoomTypeId = 1, HotelId = 1, Name = "Standard Room", Description = "Comfortable room with essential amenities", Capacity = 2, BasePrice = 1500000m, CreatedDate = seedDate },
            new RoomType { RoomTypeId = 2, HotelId = 1, Name = "Deluxe Room", Description = "Spacious room with city view", Capacity = 2, BasePrice = 2500000m, CreatedDate = seedDate },
            new RoomType { RoomTypeId = 3, HotelId = 1, Name = "Executive Suite", Description = "Luxury suite with living area", Capacity = 4, BasePrice = 4500000m, CreatedDate = seedDate },
            // Hotel 2 - Beach Paradise Resort
            new RoomType { RoomTypeId = 4, HotelId = 2, Name = "Garden View Room", Description = "Room overlooking tropical gardens", Capacity = 2, BasePrice = 1800000m, CreatedDate = seedDate },
            new RoomType { RoomTypeId = 5, HotelId = 2, Name = "Ocean View Room", Description = "Room with stunning sea views", Capacity = 2, BasePrice = 2800000m, CreatedDate = seedDate },
            new RoomType { RoomTypeId = 6, HotelId = 2, Name = "Beach Villa", Description = "Private villa with beach access", Capacity = 6, BasePrice = 8000000m, CreatedDate = seedDate },
            // Hotel 3 - Mountain View Lodge
            new RoomType { RoomTypeId = 7, HotelId = 3, Name = "Cozy Room", Description = "Warm and cozy room with mountain view", Capacity = 2, BasePrice = 1200000m, CreatedDate = seedDate },
            new RoomType { RoomTypeId = 8, HotelId = 3, Name = "Family Room", Description = "Large room for families", Capacity = 4, BasePrice = 2000000m, CreatedDate = seedDate }
        );

        // Seed Rooms
        modelBuilder.Entity<Room>().HasData(
            // Hotel 1 Rooms
            new Room { RoomId = 1, RoomTypeId = 1, RoomNumber = "101", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 2, RoomTypeId = 1, RoomNumber = "102", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 3, RoomTypeId = 2, RoomNumber = "201", Floor = 2, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 4, RoomTypeId = 2, RoomNumber = "202", Floor = 2, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 5, RoomTypeId = 3, RoomNumber = "301", Floor = 3, Status = "Available", CreatedDate = seedDate },
            // Hotel 2 Rooms
            new Room { RoomId = 6, RoomTypeId = 4, RoomNumber = "A101", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 7, RoomTypeId = 4, RoomNumber = "A102", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 8, RoomTypeId = 5, RoomNumber = "B201", Floor = 2, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 9, RoomTypeId = 5, RoomNumber = "B202", Floor = 2, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 10, RoomTypeId = 6, RoomNumber = "V01", Floor = 1, Status = "Available", CreatedDate = seedDate },
            // Hotel 3 Rooms
            new Room { RoomId = 11, RoomTypeId = 7, RoomNumber = "M101", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 12, RoomTypeId = 7, RoomNumber = "M102", Floor = 1, Status = "Available", CreatedDate = seedDate },
            new Room { RoomId = 13, RoomTypeId = 8, RoomNumber = "M201", Floor = 2, Status = "Available", CreatedDate = seedDate }
        );

        // Seed Room Amenities
        modelBuilder.Entity<RoomAmenity>().HasData(
            // Standard rooms have basic amenities
            new RoomAmenity { RoomId = 1, AmenityId = 1 }, // WiFi
            new RoomAmenity { RoomId = 1, AmenityId = 2 }, // AC
            new RoomAmenity { RoomId = 1, AmenityId = 3 }, // TV
            new RoomAmenity { RoomId = 2, AmenityId = 1 },
            new RoomAmenity { RoomId = 2, AmenityId = 2 },
            new RoomAmenity { RoomId = 2, AmenityId = 3 },
            // Deluxe rooms have more amenities
            new RoomAmenity { RoomId = 3, AmenityId = 1 },
            new RoomAmenity { RoomId = 3, AmenityId = 2 },
            new RoomAmenity { RoomId = 3, AmenityId = 3 },
            new RoomAmenity { RoomId = 3, AmenityId = 4 }, // Mini Bar
            new RoomAmenity { RoomId = 3, AmenityId = 5 }, // Safe
            // Suites have all amenities
            new RoomAmenity { RoomId = 5, AmenityId = 1 },
            new RoomAmenity { RoomId = 5, AmenityId = 2 },
            new RoomAmenity { RoomId = 5, AmenityId = 3 },
            new RoomAmenity { RoomId = 5, AmenityId = 4 },
            new RoomAmenity { RoomId = 5, AmenityId = 5 },
            new RoomAmenity { RoomId = 5, AmenityId = 6 },
            new RoomAmenity { RoomId = 5, AmenityId = 7 }
        );
    }
}
