﻿namespace Marketplace.BLL.DTO.Product;

public class UpdateProductDto
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
}