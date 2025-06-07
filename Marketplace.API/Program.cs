using Marketplace.BBL.Configurations;
using Marketplace.BBL.Services;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Helpers;
using Marketplace.DAL.Repositories;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly); // або typeof(Program).Assembly
builder.Services.AddAutoMapper(typeof(StoreProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ReviewProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
builder.Services.AddAutoMapper(typeof(OrderItemProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ISortHelper<Product>, SortHelper<Product>>();



builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); 

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MarketplaceDbContext>();
    DbSeeder.Seed(context);
}

app.Run();