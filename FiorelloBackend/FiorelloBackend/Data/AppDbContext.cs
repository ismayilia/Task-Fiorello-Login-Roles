using FiorelloBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
            public DbSet<Slider> Sliders { get; set; }
            public DbSet<SliderInfo> SliderInfos { get; set; }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<ProductImage> ProductImages { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<HomeAbout> HomeAbouts { get; set; }
            public DbSet<HomeAboutIcon> HomeAboutIcons { get; set; }
            public DbSet<Expert> Experts { get; set; }
            public DbSet<Subscribe> Subscribes { get; set; }
            public DbSet<Say> Says { get; set; }
            public DbSet<Instagram> Instagrams { get; set; }
            public DbSet<Setting> Settings { get; set; }
            public DbSet<CategoryArchive> CategoArchives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(m => !m.SoftDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(m => !m.SoftDeleted);
            modelBuilder.Entity<Blog>().HasQueryFilter(m => !m.SoftDeleted);
            modelBuilder.Entity<Slider>().HasQueryFilter(m => !m.SoftDeleted);

            //modelBuilder.Entity<Setting>().HasData(
            //    new Setting
            //    {
            //        Id=3,
            //        Key = "Phone",
            //        Value = "4544454"
            //    }




            //    );
        }
    }
}
