using FluentValidation;
using Marketplace.BLL.Configurations;
using Marketplace.BLL.DTO.Auth;
using Marketplace.BLL.DTO.Category;
using Marketplace.BLL.DTO.Order;
using Marketplace.BLL.DTO.OrderItem;
using Marketplace.BLL.DTO.Product;
using Marketplace.BLL.DTO.Review;
using Marketplace.BLL.DTO.Store;
using Marketplace.BLL.DTO.User;
using Marketplace.BLL.Services;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.BLL.Validators.Auth;
using Marketplace.BLL.Validators.Category;
using Marketplace.BLL.Validators.Order;
using Marketplace.BLL.Validators.OrderItem;
using Marketplace.BLL.Validators.Product;
using Marketplace.BLL.Validators.Review;
using Marketplace.BLL.Validators.Store;
using Marketplace.BLL.Validators.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Marketplace.BLL;

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
        
        services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
        
        services.AddScoped<IValidator<ForgotPasswordRequestDto>, ForgotPasswordRequestDtoValidator>();
        services.AddScoped<IValidator<ResetPasswordRequestDto>, ResetPasswordRequestDtoValidator>();
        
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        

        services.AddHttpContextAccessor();
        
        return services;
    }
}