using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Marketplace.DAL.Data;

public class MarketplaceDbContextFactory : IDesignTimeDbContextFactory<MarketplaceDbContext>
{
    public MarketplaceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MarketplaceDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS,1433;Database=Marketplace;Trusted_Connection=True;TrustServerCertificate=True;");

        return new MarketplaceDbContext(optionsBuilder.Options);
    }
}