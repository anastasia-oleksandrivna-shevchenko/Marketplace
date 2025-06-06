﻿namespace Marketplace.DAL.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; } 
    public Product Product { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}