using Microsoft.EntityFrameworkCore;
using ShopHunter.BDConn;
using ShopHunter.Models.BDConn;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }
    public DbSet<PictureProduct> ProductPictures { get; set; }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.CategoryProduct)
            .WithOne(c => c.Product)
            .HasForeignKey<Product>(p => p.CategoryProduct_ID);

        // Связь "один-к-одному" для картинки
        modelBuilder.Entity<Product>()
            .HasOne(p => p.PictureProduct)
            .WithOne(pp => pp.Product)
            .HasForeignKey<Product>(p => p.Picture_ID);

        // Дополнительные настройки для других сущностей
        modelBuilder.Entity<CategoryProduct>().HasKey(c => c.ID_CategoryProduct);
        modelBuilder.Entity<Product>().HasKey(p => p.ID_Product);
        modelBuilder.Entity<PictureProduct>().HasKey(pp => pp.ID_PictureProduct);

        modelBuilder.Entity<Customers>()
            .HasOne(c => c.User) // Указываем, что у Customer есть один User
            .WithOne(u => u.Customer) // Указываем, что у User есть один Customer
            .HasForeignKey<Customers>(c => c.Users_ID); // Указываем внешний ключ
        modelBuilder.Entity<Orders>().HasNoKey();
        modelBuilder.Entity<Roles>().HasNoKey();
        modelBuilder.Entity<Users>().HasKey(p => p.ID_Users);
    }
}