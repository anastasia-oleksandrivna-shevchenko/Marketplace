namespace Marketplace.DAL.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    
    public ICollection<Product> Products { get; set; }
}