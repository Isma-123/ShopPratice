

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using StoryDates.cs.BussinessEntities;


namespace StoryDataBase.cs.Context
{
    public partial class StoryContext : DbContext
    {
        public StoryContext(DbContextOptions options) : base(options)
        {
        }   


        #region BussinessEntities
        public DbSet<User> user { get; set; }   
        public DbSet<Order> order { get; set; }
        public DbSet<OrderDetails> OrderDetails  { get; set; } 
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Products { get; set; }    
        public DbSet<ProductCategory> ProductCategories { get; set; }   
        #endregion

        //Db en In memory 
       
        public StoryContext() { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configurations of the entities Orm
            modelBuilder.Entity<User>().ToTable("User").HasKey(s => s.UserId);
            modelBuilder.Entity<Product>().ToTable("Product").HasKey(d => d.ProductId);
            modelBuilder.Entity<Order>().ToTable("Order").HasKey(b => b.OrderId);
            modelBuilder.Entity<OrderDetails>().ToTable("OrderDetails").HasKey(v => v.orderDetailsId);
            modelBuilder.Entity<Category>().ToTable("Category").HasKey(c =>  c.CategoryId);
            modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });


            modelBuilder.Entity<User>()  // users configuration
             .HasMany(s => s.Orders)
             .WithOne(d => d.User)
             .HasForeignKey(t => t.OrderId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()   // Order configuration
            .HasMany(s => s.Details)
            .WithOne(d => d.Order)
            .HasForeignKey(t => t.orderDetailsId);

            modelBuilder.Entity<Order>()
            .HasOne(s => s.User)
            .WithMany(d => d.Orders)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
            .HasOne(s => s.product)
            .WithMany(d => d.productCategories)
            .HasForeignKey(a => a.ProductId);


            modelBuilder.Entity<ProductCategory>()
             .HasOne(s => s.category)
             .WithMany(d => d.productCategories)
             .HasForeignKey(a => a.CategoryId);

         
            base.OnModelCreating(modelBuilder);
        }

    }

}
