using FluentValidation;
using Marketplace.BBL.Configurations;
using Marketplace.BBL.DTO.Auth;
using Marketplace.BBL.DTO.Category;
using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.DTO.OrderItem;
using Marketplace.BBL.DTO.Product;
using Marketplace.BBL.DTO.Review;
using Marketplace.BBL.DTO.Store;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.BBL.Validators.Auth;
using Marketplace.BBL.Validators.Category;
using Marketplace.BBL.Validators.Order;
using Marketplace.BBL.Validators.OrderItem;
using Marketplace.BBL.Validators.Product;
using Marketplace.BBL.Validators.Review;
using Marketplace.BBL.Validators.Store;
using Marketplace.BBL.Validators.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Marketplace.BBL;

public static class BblDependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
        services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
        services.AddScoped<IValidator<RefreshTokenRequestDto>, RefreshTokenRequestDtoValidator>();

        services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryDtoValidator>();
        services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryDtoValidator>();
        
        services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
        services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
        
        services.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
        services.AddScoped<IValidator<UpdateOrderStatusDto>, UpdateOrderDtoValidator>();
        
        services.AddScoped<IValidator<CreateOrderItemDto>, CreateOrderItemDtoValidator>();
        services.AddScoped<IValidator<UpdateOrderItemDto>, UpdateOrderItemDtoValidator>();
        
        services.AddScoped<IValidator<CreateReviewDto>, CreateReviewDtoValidator>();
        services.AddScoped<IValidator<UpdateReviewDto>, UpdateReviewDtoValidator>();
        
        services.AddScoped<IValidator<CreateStoreDto>, CreateStoreDtoValidator>();
        services.AddScoped<IValidator<UpdateStoreDto>, UpdateStoreDtoValidator>();
        
        services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
        
        services.AddScoped<IValidator<ForgotPasswordRequestDto>, ForgotPasswordRequestDtoValidator>();
        services.AddScoped<IValidator<ResetPasswordRequestDto>, ResetPasswordRequestDtoValidator>();
        
        services.AddAutoMapper(typeof(UserProfile).Assembly);

        services.AddHttpContextAccessor();
        
        return services;
    }
}