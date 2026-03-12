using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClunyApi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<OrderSelectedOption> OrderSelectedOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderSelectedOption>()
                .Property(s => s.PriceAtPurchase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Option>()
                .Property(o => o.PriceDelta)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>().HasData(
                // ---------------- Pastries ----------------
                new Product { Id = 1, Name = "Butter Croissant", Description = "Classic French butter croissant baked fresh daily with flaky golden layers.", Price = 5.15m, CategoryId = 1, StockQuantity = 0 },
                new Product { Id = 2, Name = "Chocolate Croissant", Description = "Flaky croissant filled with premium dark chocolate batons.", Price = 5.75m, CategoryId = 1, StockQuantity = 0 },
                new Product { Id = 3, Name = "Almond Croissant", Description = "Croissant filled with almond cream, topped with toasted almonds and powdered sugar.", Price = 6.25m, CategoryId = 1, StockQuantity = 0 },
                new Product { Id = 4, Name = "Blueberry Muffin", Description = "Moist muffin packed with fresh blueberries and topped with crumble.", Price = 4.50m, CategoryId = 1, StockQuantity = 0 },
                new Product { Id = 5, Name = "Cinnamon Roll", Description = "Soft rolled pastry with cinnamon sugar filling and cream cheese icing.", Price = 5.95m, CategoryId = 1, StockQuantity = 0 },

                // ---------------- Cold Beverages ----------------
                new Product { Id = 6, Name = "Iced Latte", Description = "Espresso combined with chilled milk and served over ice.", Price = 5.45m, CategoryId = 2, StockQuantity = 0 },
                new Product { Id = 7, Name = "Cold Brew Coffee", Description = "Slow steeped coffee served cold for smooth and bold flavour.", Price = 4.95m, CategoryId = 2, StockQuantity = 0 },
                new Product { Id = 8, Name = "Iced Matcha Latte", Description = "Premium Japanese matcha whisked with milk and served over ice.", Price = 6.25m, CategoryId = 2, StockQuantity = 0 },
                new Product { Id = 9, Name = "Fresh Lemonade", Description = "Freshly squeezed lemon juice with lightly sweetened chilled water.", Price = 4.75m, CategoryId = 2, StockQuantity = 0 },
                new Product { Id = 10, Name = "Iced Chai Latte", Description = "Spiced chai tea concentrate blended with cold milk and ice.", Price = 5.75m, CategoryId = 2, StockQuantity = 0 },

                // ---------------- Hot Beverages ----------------
                new Product { Id = 11, Name = "Espresso", Description = "Rich and concentrated espresso shot made from premium roasted beans.", Price = 3.25m, CategoryId = 3, StockQuantity = 0 },
                new Product { Id = 12, Name = "Americano", Description = "Espresso diluted with hot water for a smooth coffee experience.", Price = 3.75m, CategoryId = 3, StockQuantity = 0 },
                new Product { Id = 13, Name = "Cappuccino", Description = "Equal parts espresso, steamed milk, and silky foam.", Price = 5.15m, CategoryId = 3, StockQuantity = 0 },
                new Product { Id = 14, Name = "Flat White", Description = "Double espresso with velvety microfoam milk.", Price = 5.45m, CategoryId = 3, StockQuantity = 0 },
                new Product { Id = 15, Name = "Hot Chocolate", Description = "Steamed milk blended with rich melted chocolate and topped with foam.", Price = 4.95m, CategoryId = 3, StockQuantity = 0 },

                // ---------------- Bread ----------------
                new Product { Id = 16, Name = "Sourdough Loaf", Description = "Naturally fermented sourdough bread with crispy crust and airy crumb.", Price = 7.95m, CategoryId = 4, StockQuantity = 0 },
                new Product { Id = 17, Name = "Baguette", Description = "Traditional French baguette with crunchy crust and soft interior.", Price = 4.25m, CategoryId = 4, StockQuantity = 0 },
                new Product { Id = 18, Name = "Whole Wheat Bread", Description = "Healthy whole wheat loaf baked fresh daily.", Price = 6.75m, CategoryId = 4, StockQuantity = 0 },
                new Product { Id = 19, Name = "Multigrain Bread", Description = "Hearty loaf made with mixed grains and seeds.", Price = 7.25m, CategoryId = 4, StockQuantity = 0 },

                // ---------------- Sandwiches ----------------
                new Product { Id = 20, Name = "Turkey & Swiss Sandwich", Description = "Roasted turkey breast, Swiss cheese, lettuce, and tomato on sourdough.", Price = 9.95m, CategoryId = 5, StockQuantity = 0 },
                new Product { Id = 21, Name = "Chicken Pesto Sandwich", Description = "Grilled chicken breast with basil pesto, mozzarella, and arugula.", Price = 10.75m, CategoryId = 5, StockQuantity = 0 },
                new Product { Id = 22, Name = "Grilled Cheese Sandwich", Description = "Melted cheddar and mozzarella grilled on buttered artisan bread.", Price = 8.25m, CategoryId = 5, StockQuantity = 0 },
                new Product { Id = 23, Name = "Veggie Sandwich", Description = "Roasted vegetables, hummus, spinach, and avocado on multigrain bread.", Price = 9.45m, CategoryId = 5, StockQuantity = 0 },

                // ---------------- Pantry ----------------
                new Product { Id = 24, Name = "House Coffee Beans (340g)", Description = "Medium roast whole bean coffee with chocolate and caramel notes.", Price = 16.95m, CategoryId = 6, StockQuantity = 0 },
                new Product { Id = 25, Name = "Artisan Honey Jar", Description = "Locally sourced raw wildflower honey.", Price = 12.50m, CategoryId = 6, StockQuantity = 0 },
                new Product { Id = 26, Name = "Strawberry Jam", Description = "Small batch strawberry jam made with fresh fruit.", Price = 8.95m, CategoryId = 6, StockQuantity = 0 },
                new Product { Id = 27, Name = "Granola Mix", Description = "House-made granola with oats, nuts, seeds, and dried fruits.", Price = 9.75m, CategoryId = 6, StockQuantity = 0 }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Pastries" },
                new Category { Id = 2, Name = "Cold Beverages" },
                new Category { Id = 3, Name = "Hot Beverages" },
                new Category { Id = 4, Name = "Bread" },
                new Category { Id = 5, Name = "Sandwiches" },
                new Category { Id = 6, Name = "Pantry" }
            );


            modelBuilder.Entity<OptionGroup>().HasData(
                new OptionGroup
                {
                    Id = 1,
                    Name = "Milk Options",
                    IsRequired = true,
                    DisplayOrder = 1
                }
            );

            modelBuilder.Entity<Option>().HasData(
                new Option { Id = 1, OptionGroupId = 1, Name = "2% Milk", PriceDelta = 0.00m, IsDefault = true, DisplayOrder = 1 },
                new Option { Id = 2, OptionGroupId = 1, Name = "Soy Milk", PriceDelta = 0.75m, IsDefault = false, DisplayOrder = 2 },
                new Option { Id = 3, OptionGroupId = 1, Name = "Almond Milk", PriceDelta = 0.75m, IsDefault = false, DisplayOrder = 3 },
                new Option { Id = 4, OptionGroupId = 1, Name = "Oat Milk", PriceDelta = 0.75m, IsDefault = false, DisplayOrder = 4 }
            );

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OptionGroups)
                .WithMany(g => g.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductOptionGroups",
                    j => j.HasOne<OptionGroup>().WithMany().HasForeignKey("OptionGroupId").HasConstraintName("FK_ProductOptionGroups_OptionGroups_OptionGroupId"),
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK_ProductOptionGroups_Products_ProductId"),
                    j =>
                    {
                        j.HasKey("ProductId", "OptionGroupId");
                        j.ToTable("ProductOptionGroups");
                    }
                );

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("ProductOptionGroups").HasData(
                new { ProductId = 6, OptionGroupId = 1 },
                new { ProductId = 7, OptionGroupId = 1 },
                new { ProductId = 8, OptionGroupId = 1 },
                new { ProductId = 9, OptionGroupId = 1 },
                new { ProductId = 10, OptionGroupId = 1 },
                new { ProductId = 11, OptionGroupId = 1 },
                new { ProductId = 12, OptionGroupId = 1 },
                new { ProductId = 13, OptionGroupId = 1 },
                new { ProductId = 14, OptionGroupId = 1 },
                new { ProductId = 15, OptionGroupId = 1 }
            );
        }

    }
}
