using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Top5.Data;
using Top5.Models;

namespace Top5.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            



            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Mains" },
                new Category { Id = 2, Name = "Soups" },
                new Category { Id = 3, Name = "Sides" }
            );

            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    Id = 1,
                    Name = "Stew peas",
                    Price = 1500,
                    CategoryId = 1
                },
                new Dish
                {
                    Id = 2,
                    Name = "Escovitch fish",
                    Price = 1800,
                    IsSpicy = true,
                    CategoryId = 1
                },
                new Dish
                {
                    Id = 3,
                    Name = "Mannish water",
                    Price = 900,
                    IsSpicy = true,
                    CategoryId = 2
                });

            modelBuilder.Entity<Dish>().ToTable("Dish");

            modelBuilder.Entity<Dish>()
                .HasIndex(dish => dish.Name)
                .IsUnique();
        }
    }
}