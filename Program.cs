using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Auramode.Data;
using Auramode.Models;
using Auramode.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure SQLite Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=auramode.db"));

// Configure Services
builder.Services.AddHttpClient<PaystackService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Auto-migrate and seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    db.SeedData();
}

app.UseCors("AllowAll");

// Serve files from root directory
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath),
    RequestPath = ""
});

// Root redirect
app.MapGet("/", () => Results.Redirect("/index.html"));

// Health check endpoint
app.MapGet("/api/health", () => Results.Ok(new { status = "online", brand = "AURAMODE", timestamp = DateTime.UtcNow }));

// Paystack Config
app.MapGet("/api/config/paystack", (PaystackService paystack) =>
{
    return Results.Ok(new { publicKey = paystack.GetPublicKey() });
});

// Products API
app.MapGet("/api/products", async (string? category, string? collection, string? search, string? badge, AppDbContext db) =>
{
    var query = db.Products.AsQueryable();

    if (!string.IsNullOrWhiteSpace(category))
    {
        query = query.Where(p => p.Category.ToLower() == category.ToLower());
    }

    if (!string.IsNullOrWhiteSpace(collection))
    {
        query = query.Where(p => p.Collection.ToLower() == collection.ToLower());
    }

    if (!string.IsNullOrWhiteSpace(badge))
    {
        query = query.Where(p => p.Badge.ToLower() == badge.ToLower());
    }

    if (!string.IsNullOrWhiteSpace(search))
    {
        var s = search.ToLower();
        query = query.Where(p => p.Name.ToLower().Contains(s) || p.Subtitle.ToLower().Contains(s) || p.Description.ToLower().Contains(s));
    }

    var list = await query.ToListAsync();
    var result = list.Select(p => new
    {
        id = p.Id,
        name = p.Name,
        subtitle = p.Subtitle,
        price = p.Price,
        category = p.Category,
        collection = p.Collection,
        badge = p.Badge,
        image = p.Image,
        gallery = JsonSerializer.Deserialize<List<string>>(p.GalleryJson) ?? new(),
        colors = JsonSerializer.Deserialize<List<string>>(p.ColorsJson) ?? new(),
        sizes = JsonSerializer.Deserialize<List<string>>(p.SizesJson) ?? new(),
        inStock = p.InStock,
        stockQuantity = p.StockQuantity,
        description = p.Description
    });

    return Results.Ok(result);
});

app.MapGet("/api/products/{id}", async (string id, AppDbContext db) =>
{
    var p = await db.Products.FindAsync(id);
    if (p == null) return Results.NotFound(new { message = "Product not found" });

    return Results.Ok(new
    {
        id = p.Id,
        name = p.Name,
        subtitle = p.Subtitle,
        price = p.Price,
        category = p.Category,
        collection = p.Collection,
        badge = p.Badge,
        image = p.Image,
        gallery = JsonSerializer.Deserialize<List<string>>(p.GalleryJson) ?? new(),
        colors = JsonSerializer.Deserialize<List<string>>(p.ColorsJson) ?? new(),
        sizes = JsonSerializer.Deserialize<List<string>>(p.SizesJson) ?? new(),
        inStock = p.InStock,
        stockQuantity = p.StockQuantity,
        description = p.Description
    });
});

// Auth API - Register
app.MapPost("/api/auth/register", async (RegisterRequest req, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
    {
        return Results.BadRequest(new { message = "Email and password are required." });
    }

    string normalizedEmail = req.Email.Trim().ToLowerInvariant();
    var existing = await db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    if (existing != null)
    {
        return Results.BadRequest(new { message = "An account with this email already exists." });
    }

    var (hash, salt) = AuthService.HashPassword(req.Password);
    var user = new UserEntity
    {
        Email = normalizedEmail,
        PasswordHash = hash,
        Salt = salt,
        FirstName = req.FirstName.Trim(),
        LastName = req.LastName.Trim(),
        Phone = req.Phone?.Trim() ?? string.Empty
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        success = true,
        user = new
        {
            id = user.Id,
            email = user.Email,
            name = $"{user.FirstName} {user.LastName}".Trim(),
            firstName = user.FirstName,
            lastName = user.LastName,
            phone = user.Phone,
            address = user.Address,
            city = user.City,
            state = user.State,
            postalCode = user.PostalCode,
            loggedIn = true
        }
    });
});

// Auth API - Login
app.MapPost("/api/auth/login", async (LoginRequest req, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
    {
        return Results.BadRequest(new { message = "Email and password are required." });
    }

    string normalizedEmail = req.Email.Trim().ToLowerInvariant();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    if (user == null || !AuthService.VerifyPassword(req.Password, user.PasswordHash, user.Salt))
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new
    {
        success = true,
        user = new
        {
            id = user.Id,
            email = user.Email,
            name = $"{user.FirstName} {user.LastName}".Trim(),
            firstName = user.FirstName,
            lastName = user.LastName,
            phone = user.Phone,
            address = user.Address,
            city = user.City,
            state = user.State,
            postalCode = user.PostalCode,
            loggedIn = true
        }
    });
});

// Auth API - Get Profile
app.MapGet("/api/auth/me", async (string email, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(email)) return Results.BadRequest();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email.Trim().ToLowerInvariant());
    if (user == null) return Results.NotFound();

    return Results.Ok(new
    {
        id = user.Id,
        email = user.Email,
        name = $"{user.FirstName} {user.LastName}".Trim(),
        firstName = user.FirstName,
        lastName = user.LastName,
        phone = user.Phone,
        address = user.Address,
        city = user.City,
        state = user.State,
        postalCode = user.PostalCode
    });
});

// Auth API - Update Profile
app.MapPut("/api/auth/profile", async (UpdateProfileRequest req, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(req.Email)) return Results.BadRequest();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email.Trim().ToLowerInvariant());
    if (user == null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(req.FirstName)) user.FirstName = req.FirstName.Trim();
    if (!string.IsNullOrWhiteSpace(req.LastName)) user.LastName = req.LastName.Trim();
    if (!string.IsNullOrWhiteSpace(req.Phone)) user.Phone = req.Phone.Trim();
    if (!string.IsNullOrWhiteSpace(req.Address)) user.Address = req.Address.Trim();
    if (!string.IsNullOrWhiteSpace(req.City)) user.City = req.City.Trim();
    if (!string.IsNullOrWhiteSpace(req.State)) user.State = req.State.Trim();
    if (!string.IsNullOrWhiteSpace(req.PostalCode)) user.PostalCode = req.PostalCode.Trim();

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        success = true,
        user = new
        {
            id = user.Id,
            email = user.Email,
            name = $"{user.FirstName} {user.LastName}".Trim(),
            firstName = user.FirstName,
            lastName = user.LastName,
            phone = user.Phone,
            address = user.Address,
            city = user.City,
            state = user.State,
            postalCode = user.PostalCode,
            loggedIn = true
        }
    });
});

// Orders API - List
app.MapGet("/api/orders", async (string? email, string? userId, AppDbContext db) =>
{
    var query = db.Orders.AsQueryable();
    if (!string.IsNullOrWhiteSpace(email))
    {
        query = query.Where(o => o.Email == email.Trim().ToLowerInvariant());
    }
    if (!string.IsNullOrWhiteSpace(userId))
    {
        query = query.Where(o => o.UserId == userId);
    }

    var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
    var result = orders.Select(o => new
    {
        orderNo = o.OrderNo,
        status = o.Status,
        date = o.CreatedAt.ToString("dd/MM/yyyy"),
        total = o.Total,
        paymentMethod = o.PaymentMethod,
        paymentReference = o.PaymentReference,
        items = JsonSerializer.Deserialize<List<OrderItemDto>>(o.ItemsJson) ?? new(),
        shipping = new
        {
            firstName = o.FirstName,
            lastName = o.LastName,
            phone = o.Phone,
            address = o.Address,
            city = o.City,
            state = o.State,
            delivery = o.DeliveryMethod
        }
    });

    return Results.Ok(result);
});

// Orders API - Single
app.MapGet("/api/orders/{orderNo}", async (string orderNo, AppDbContext db) =>
{
    var o = await db.Orders.FirstOrDefaultAsync(x => x.OrderNo == orderNo);
    if (o == null) return Results.NotFound(new { message = "Order not found" });

    return Results.Ok(new
    {
        orderNo = o.OrderNo,
        status = o.Status,
        date = o.CreatedAt.ToString("dd/MM/yyyy"),
        total = o.Total,
        paymentMethod = o.PaymentMethod,
        paymentReference = o.PaymentReference,
        items = JsonSerializer.Deserialize<List<OrderItemDto>>(o.ItemsJson) ?? new(),
        shipping = new
        {
            firstName = o.FirstName,
            lastName = o.LastName,
            phone = o.Phone,
            address = o.Address,
            city = o.City,
            state = o.State,
            delivery = o.DeliveryMethod
        }
    });
});

// Orders API - Create
app.MapPost("/api/orders", async (CreateOrderRequest req, AppDbContext db) =>
{
    if (req.Items == null || !req.Items.Any())
    {
        return Results.BadRequest(new { message = "Order must have at least one item." });
    }

    string orderNo = Math.Floor(10000000 + Random.Shared.NextDouble() * 90000000).ToString();
    var order = new OrderEntity
    {
        OrderNo = orderNo,
        UserId = req.UserId,
        Email = req.Email.Trim().ToLowerInvariant(),
        FirstName = req.FirstName.Trim(),
        LastName = req.LastName.Trim(),
        Phone = req.Phone.Trim(),
        Address = req.Address.Trim(),
        City = req.City.Trim(),
        State = req.State.Trim(),
        DeliveryMethod = req.DeliveryMethod,
        PaymentMethod = req.PaymentMethod,
        PaymentReference = req.PaymentReference,
        Status = req.PaymentMethod == "paystack" && !string.IsNullOrEmpty(req.PaymentReference) ? "PAID" : "PROCESSING",
        Total = req.Total,
        ItemsJson = JsonSerializer.Serialize(req.Items),
        CreatedAt = DateTime.UtcNow
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        success = true,
        orderNo = order.OrderNo,
        status = order.Status,
        total = order.Total,
        date = order.CreatedAt.ToString("dd/MM/yyyy")
    });
});

// Paystack Payment Initialize
app.MapPost("/api/payments/paystack/initialize", async (CreateOrderRequest req, PaystackService paystack, HttpContext http) =>
{
    string reference = $"AURA_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{Random.Shared.Next(1000, 9999)}";
    string callbackUrl = $"{http.Request.Scheme}://{http.Request.Host}/order-confirmation.html?reference={reference}";

    var result = await paystack.InitializeTransactionAsync(req.Email, req.Total, reference, callbackUrl);
    return Results.Ok(new
    {
        success = result.Success,
        reference = result.Reference,
        authorizationUrl = result.AuthorizationUrl,
        accessCode = result.AccessCode,
        publicKey = paystack.GetPublicKey()
    });
});

// Paystack Payment Verify
app.MapPost("/api/payments/paystack/verify", async (PaymentVerifyRequest req, PaystackService paystack, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(req.Reference))
    {
        return Results.BadRequest(new { message = "Reference is required" });
    }

    bool verified = await paystack.VerifyTransactionAsync(req.Reference);
    if (!verified)
    {
        return Results.BadRequest(new { success = false, message = "Payment verification failed" });
    }

    if (!string.IsNullOrWhiteSpace(req.OrderNo))
    {
        var order = await db.Orders.FirstOrDefaultAsync(o => o.OrderNo == req.OrderNo);
        if (order != null)
        {
            order.Status = "PAID";
            order.PaymentReference = req.Reference;
            await db.SaveChangesAsync();
        }
    }

    return Results.Ok(new
    {
        success = true,
        reference = req.Reference,
        message = "Payment verified successfully"
    });
});

app.Run();
