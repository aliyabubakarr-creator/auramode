namespace Auramode.Models;

public class ProductEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Collection { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string GalleryJson { get; set; } = "[]";
    public string ColorsJson { get; set; } = "[]";
    public string SizesJson { get; set; } = "[]";
    public bool InStock { get; set; } = true;
    public int StockQuantity { get; set; } = 50;
    public string Description { get; set; } = string.Empty;
}

public class UserEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class OrderEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string OrderNo { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string DeliveryMethod { get; set; } = "standard";
    public string PaymentMethod { get; set; } = "paystack";
    public string Status { get; set; } = "PROCESSING";
    public decimal Total { get; set; }
    public string ItemsJson { get; set; } = "[]";
    public string? PaymentReference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class UpdateProfileRequest
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class CreateOrderRequest
{
    public string? UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string DeliveryMethod { get; set; } = "standard";
    public string PaymentMethod { get; set; } = "paystack";
    public decimal Total { get; set; }
    public string? PaymentReference { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public string Image { get; set; } = string.Empty;
}

public class PaymentVerifyRequest
{
    public string Reference { get; set; } = string.Empty;
    public string? OrderNo { get; set; }
}

