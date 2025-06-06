﻿using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class ProductRepository: GenericRepository<Product>, IProductRepository
{
    public ProductRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Product>> FindProductsByCategoryIdAsync(int categoryId)
    {
        return await _dbSet
            .Where(o => o.CategoryId == categoryId)
            .Include(p => p.Store)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByStoreIdAsync(int storeId)
    {
        return await _dbSet
            .Where(o => o.StoreId == storeId)
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByNameAsync(string name)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(name))
            .Include(p => p.Store)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .Include(p => p.Store)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsSortedByPriceAsync(bool ascending = true)
    {
        return await _dbSet
            .OrderBy(p => ascending? p.Price : -p.Price)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindProductsSortedByRatingAsync(bool ascending = false)
    {
        return await _dbSet
            .Include(p => p.Reviews)
            .OrderBy(p => ascending 
                ? p.Reviews.Select(r=>r.Rating).DefaultIfEmpty(0).Average() 
                : -p.Reviews.Select(r => r.Rating).Average())
            .ToListAsync();
    }
}