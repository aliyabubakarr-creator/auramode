using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Auramode.Models;
using Auramode.Services;

namespace Auramode.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    public void SeedData()
    {
        if (!Products.Any())
        {
            var seedProducts = new List<ProductEntity>
            {
                new()
                {
                    Id = "mushroom-cosmic-white",
                    Name = "Cosmic Tee",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 2",
                    Badge = "NEW",
                    Image = "assets/product-cosmic-white.png",
                    GalleryJson = JsonSerializer.Serialize(new[]
                    {
                        "assets/product-detail-front.png",
                        "assets/product-detail-models.png",
                        "assets/product-detail-closeup.png",
                        "assets/product-detail-back.png"
                    }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "White", "Black" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L", "XL", "2XL" }),
                    InStock = true,
                    StockQuantity = 45,
                    Description = "Signature graphic tee crafted from 280GSM heavyweight combed organic cotton. Features screenprinted cosmic mushroom motifs and custom relaxed boxy fit."
                },
                new()
                {
                    Id = "mushroom-duo-models",
                    Name = "Campaign Tee",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 1",
                    Badge = "NEW",
                    Image = "assets/product-models-white.png",
                    GalleryJson = JsonSerializer.Serialize(new[]
                    {
                        "assets/product-detail-models.png",
                        "assets/product-detail-front.png",
                        "assets/product-detail-closeup.png",
                        "assets/product-detail-back.png"
                    }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "White", "Black" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L", "XL" }),
                    InStock = true,
                    StockQuantity = 30,
                    Description = "Campaign edition graphic tee featuring double front placement and ribbed crewneck."
                },
                new()
                {
                    Id = "auramode-trucker-hat",
                    Name = "Trucker Cap",
                    Subtitle = "Headwear",
                    Price = 15000,
                    Category = "accessories",
                    Collection = "SZN 3",
                    Badge = "",
                    Image = "assets/product-cap.png",
                    GalleryJson = JsonSerializer.Serialize(new[] { "assets/product-cap.png" }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "Grey/White", "Black" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "One Size" }),
                    InStock = true,
                    StockQuantity = 50,
                    Description = "Branded trucker hat with mesh back, curved brim and embroidered AURAMODE front script."
                },
                new()
                {
                    Id = "mushroom-seated-black",
                    Name = "Vintage Black Tee",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 1",
                    Badge = "NEW",
                    Image = "assets/product-seated-black.png",
                    GalleryJson = JsonSerializer.Serialize(new[]
                    {
                        "assets/product-seated-black.png",
                        "assets/product-boxlogo-black.png"
                    }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "Black", "White" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L", "XL" }),
                    InStock = true,
                    StockQuantity = 25,
                    Description = "Washed vintage black heavy jersey with photographic center back print and subtle breast pocket print."
                },
                new()
                {
                    Id = "auramode-boxlogo-black",
                    Name = "Box Logo Tee",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 2",
                    Badge = "NEW",
                    Image = "assets/product-boxlogo-black.png",
                    GalleryJson = JsonSerializer.Serialize(new[] { "assets/product-boxlogo-black.png" }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "Black", "White" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L", "XL" }),
                    InStock = true,
                    StockQuantity = 35,
                    Description = "Minimalist brutalist box emblem screenprint on deep black heavyweight cotton."
                },
                new()
                {
                    Id = "mushroom-nature-white",
                    Name = "Botanical Tee",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 1",
                    Badge = "SOLD OUT",
                    Image = "assets/product-nature-white.png",
                    GalleryJson = JsonSerializer.Serialize(new[] { "assets/product-nature-white.png" }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "White" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L" }),
                    InStock = false,
                    StockQuantity = 0,
                    Description = "Botanical archive print white tee. Limited archival release."
                },
                new()
                {
                    Id = "target-silhouette-white",
                    Name = "Target Silhouette",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 3",
                    Badge = "SOLD OUT",
                    Image = "assets/product-target-white.png",
                    GalleryJson = JsonSerializer.Serialize(new[] { "assets/product-target-white.png" }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "White" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L" }),
                    InStock = false,
                    StockQuantity = 0,
                    Description = "Target crosshair silhouette graphic on optic white tee."
                },
                new()
                {
                    Id = "auramode-grey-heavyweight",
                    Name = "Heavyweight Grey",
                    Subtitle = "T-Shirt",
                    Price = 15000,
                    Category = "wears",
                    Collection = "SZN 4",
                    Badge = "",
                    Image = "assets/product-grey-tee.png",
                    GalleryJson = JsonSerializer.Serialize(new[] { "assets/product-grey-tee.png" }),
                    ColorsJson = JsonSerializer.Serialize(new[] { "Grey" }),
                    SizesJson = JsonSerializer.Serialize(new[] { "S", "M", "L", "XL" }),
                    InStock = true,
                    StockQuantity = 60,
                    Description = "Upcoming SZN 4 staple grey tee with oval AURAMODE chest insignia."
                }
            };

            Products.AddRange(seedProducts);
        }

        if (!Users.Any())
        {
            var (hash, salt) = AuthService.HashPassword("auramode123");
            Users.Add(new UserEntity
            {
                Id = "user-sherlock-default",
                Email = "auramodeszn@gmail.com",
                FirstName = "Sherlock",
                LastName = "P",
                PasswordHash = hash,
                Salt = salt,
                Phone = "09024462630",
                Address = "River Park Estate, Lugbe",
                City = "Abuja",
                State = "Abuja",
                PostalCode = "900001",
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!Orders.Any())
        {
            var sampleItems1 = new[]
            {
                new { name = "Cosmic Tee T-Shirt", image = "assets/product-cosmic-white.png", qty = 1, price = 15000, color = "White", size = "M" },
                new { name = "Vintage Black Tee T-Shirt", image = "assets/product-seated-black.png", qty = 1, price = 15000, color = "Black", size = "M" },
                new { name = "Box Logo Tee T-Shirt", image = "assets/product-boxlogo-black.png", qty = 1, price = 15000, color = "Black", size = "M" },
                new { name = "Trucker Cap Headwear", image = "assets/product-cap.png", qty = 1, price = 15000, color = "Grey/White", size = "One Size" }
            };

            var sampleItems2 = new[]
            {
                new { name = "Cosmic Tee T-Shirt", image = "assets/product-cosmic-white.png", qty = 1, price = 15000, color = "White", size = "L" },
                new { name = "Vintage Black Tee T-Shirt", image = "assets/product-seated-black.png", qty = 1, price = 15000, color = "Black", size = "L" }
            };

            Orders.AddRange(
                new OrderEntity
                {
                    OrderNo = "22445588",
                    UserId = "user-sherlock-default",
                    Email = "auramodeszn@gmail.com",
                    FirstName = "Sherlock",
                    LastName = "P",
                    Phone = "09024462630",
                    Address = "River Park Estate, Lugbe",
                    City = "Abuja",
                    State = "Abuja",
                    Status = "SENT",
                    Total = 60000,
                    ItemsJson = JsonSerializer.Serialize(sampleItems1),
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new OrderEntity
                {
                    OrderNo = "22445589",
                    UserId = "user-sherlock-default",
                    Email = "auramodeszn@gmail.com",
                    FirstName = "Sherlock",
                    LastName = "P",
                    Phone = "09024462630",
                    Address = "River Park Estate, Lugbe",
                    City = "Abuja",
                    State = "Abuja",
                    Status = "SENT",
                    Total = 30000,
                    ItemsJson = JsonSerializer.Serialize(sampleItems2),
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new OrderEntity
                {
                    OrderNo = "22445590",
                    UserId = "user-sherlock-default",
                    Email = "auramodeszn@gmail.com",
                    FirstName = "Sherlock",
                    LastName = "P",
                    Phone = "09024462630",
                    Address = "River Park Estate, Lugbe",
                    City = "Abuja",
                    State = "Abuja",
                    Status = "SENT",
                    Total = 60000,
                    ItemsJson = JsonSerializer.Serialize(sampleItems1),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            );
        }

        SaveChanges();
    }
}

