namespace Marketplace.Data;
using Bogus;
using System.Collections.Generic;
using Entities;
using Microsoft.EntityFrameworkCore;

public class DbSeeder
{

    public static void Seed(MarketplaceDbContext context)
    {
        context.Database.Migrate();
    
        if (!context.Categories.Any())
        {
            var categories = GenerateCategories(10);
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        if (!context.Users.Any())
        {
            var users = GenerateUsers(10);
            context.Users.AddRange(users);
            context.SaveChanges();
        }
        if (!context.Stores.Any())
        {
            var users = context.Users.ToList();
            var stores = GenerateStores(10, users);
            context.Stores.AddRange(stores);
            context.SaveChanges();
        }
        if (!context.Products.Any())
        {
            var stores = context.Stores.ToList();
            var categories = context.Categories.ToList();
            var products = GenerateProducts(10, stores, categories);
            context.Products.AddRange(products);
            context.SaveChanges();
        }
        if (!context.Review.Any())
        {
            var users = context.Users.ToList();
            var products = context.Products.ToList();
            var reviews = GenerateReviews(10, products, users);
            context.Review.AddRange(reviews);
            context.SaveChanges();
        }
        if (!context.Orders.Any())
        {
            var users = context.Users.ToList();
            var stores = context.Stores.ToList();
            var orders = GenerateOrders(10, users, stores);
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
        if (!context.OrderItems.Any())
        {
            var orders = context.Orders.ToList();
            var products = context.Products.ToList();
            var orderitems = GenerateOrderItems(10, orders, products);
            context.OrderItems.AddRange(orderitems);
            context.SaveChanges();
        }
    }
    
    public static List<User> GenerateUsers(int count = 10)
    {
        var roles = new[] { "Buyer", "Seller" };

        var faker = new Faker<User>()
            .RuleFor(u => u.UserId, f => 0) 
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.MiddleName, f => f.Random.Bool(0.5f) ? f.Name.FirstName() : null)
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password(8, false))
            .RuleFor(u => u.Role, f => f.PickRandom(roles))
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(1));

        var users = faker.Generate(count);

        return users;
    }
    
    public static List<Store> GenerateStores(int count, List<User> users)
    {
        var faker = new Faker<Store>()
            .RuleFor(s => s.StoreId, f => 0) 
            .RuleFor(s => s.UserId, f => f.PickRandom(users).UserId) 
            .RuleFor(s => s.StoreName, f => f.Company.CompanyName())
            .RuleFor(s => s.Description, f => f.Lorem.Sentence(10))
            .RuleFor(s => s.Location, f => f.Address.City())
            .RuleFor(s => s.Rating, f => f.Random.Float(0, 5))
            .RuleFor(s => s.Products, f => new List<Product>())
            .RuleFor(s => s.Orders, f => new List<Order>());

        var stores = faker.Generate(count);

        return stores;
    }
    
    public static List<Category> GenerateCategories(int count)
    {
        var faker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => 0) 
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Products, f => new List<Product>());

        var categories = faker.Generate(count);
        return categories;
    }
    
    public static List<Product> GenerateProducts(int count, List<Store> stores, List<Category> categories)
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.ProductId, f => 0) 
            .RuleFor(p => p.StoreId, f => f.PickRandom(stores).StoreId)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 1000)))
            .RuleFor(p => p.Quantity, f => f.Random.Int(0, 100))
            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).CategoryId)
            .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl(200, 200))
            .RuleFor(p => p.OrderItems, f => new List<OrderItem>())
            .RuleFor(p => p.Reviews, f => new List<Review>());

        var products = faker.Generate(count);

        return products;
    }
    
    public static List<Order> GenerateOrders(int count, List<User> users, List<Store> stores)
    {
        var orderId = 1;

        var faker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => 0)
            .RuleFor(o => o.CustomerId, f => f.PickRandom(users).UserId)
            .RuleFor(o => o.StoreId, f => f.PickRandom(stores).StoreId)
            .RuleFor(o => o.OrderDate, f => f.Date.Past(1))
            .RuleFor(o => o.Status, f => f.PickRandom(new[] { "Pending", "Shipped", "Completed" }))
            .RuleFor(o => o.TotalPrice, f => f.Finance.Amount(10, 1000))
            .RuleFor(o => o.OrderItems, f => new List<OrderItem>());

        var orders = faker.Generate(count);
        return orders;
    }
    
    public static List<OrderItem> GenerateOrderItems(int count, List<Order> orders, List<Product> products)
    {
        var orderItemId = 1;

        var faker = new Faker<OrderItem>()
            .RuleFor(oi => oi.OrderItemId, f => 0) 
            .RuleFor(oi => oi.OrderId, f => f.PickRandom(orders).OrderId)
            .RuleFor(oi => oi.ProductId, f => f.PickRandom(products).ProductId)
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(oi => oi.Price, (f, oi) =>
            {
                var product = products.Find(p => p.ProductId == oi.ProductId);
                return product != null ? product.Price * oi.Quantity : f.Finance.Amount(10, 1000);
            });
        return faker.Generate(count);
    }
    
    public static List<Review> GenerateReviews(int count, List<Product> products, List<User> users)
    {
        var reviewId = 1;

        var faker = new Faker<Review>()
            .RuleFor(r => r.ReviewId, f => 0)  
            .RuleFor(r => r.ProductId, f => f.PickRandom(products).ProductId)
            .RuleFor(r => r.UserId, f => f.PickRandom(users).UserId)
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
            .RuleFor(r => r.Comment, f => f.Lorem.Sentence(5, 10))
            .RuleFor(r => r.CreatedAt, f => f.Date.Past(1)); 

        return faker.Generate(count);
    }
    
}