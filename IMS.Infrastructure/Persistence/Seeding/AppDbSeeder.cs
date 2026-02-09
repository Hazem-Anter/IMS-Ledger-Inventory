using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence.Seeding;

public static class AppDbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        /*

        // Ensure DB is created/migrated
        await db.Database.MigrateAsync();

        // 1) Seed Warehouses
        if (!await db.Warehouses.AnyAsync())
        {
            db.Warehouses.Add(new Warehouse("Main Warehouse", "WH-MAIN"));
            db.Warehouses.Add(new Warehouse("Secondary Warehouse", "WH-SEC"));
            await db.SaveChangesAsync();
        }

        // 2) Seed Locations (for Main Warehouse)
        if (!await db.Locations.AnyAsync())
        {
            var mainWh = await db.Warehouses.FirstAsync(w => w.Code == "WH-MAIN");

            db.Locations.Add(new Location(mainWh.Id, "A-01-BIN-01"));
            db.Locations.Add(new Location(mainWh.Id, "A-01-BIN-02"));
            await db.SaveChangesAsync();
        }

        // 3) Seed Products
        if (!await db.Products.AnyAsync())
        {
            db.Products.Add(new Product("USB Keyboard", "SKU-KEY-001", "1234567890123", minStockLevel: 5));
            db.Products.Add(new Product("Wireless Mouse", "SKU-MOU-001", "1234567890456", minStockLevel: 10));
            await db.SaveChangesAsync();
        }
        */
        
    }
}
