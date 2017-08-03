using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Models;
using GlarinWood.Areas.Administrator.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GlarinWood.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }
        //OnModelCreating رابط
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region روابط جداول
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                         .WithMany(c => c.Products)
                         .HasForeignKey(p => p.CategoryId);
            });
            builder.Entity<HeaderColumnsDes>(entity =>
            {
                entity.HasOne(p => p.HeaderColumn)
                         .WithMany(c => c.HeaderColumnsDesS)
                         .HasForeignKey(p => p.HeaderColumnId);
            });
            builder.Entity<PicCollectionGallery>(entity =>
            {
                entity.HasOne(p => p.PicGallery)
                         .WithMany(c => c.PicCollectionGallerys)
                         .HasForeignKey(p => p.PicGalleryId);
            });
            builder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(p => p.Order)
                         .WithMany(c => c.OrderDetails)
                         .HasForeignKey(p => p.OrderId);
            });
            builder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(p => p.Product)
                         .WithMany(c => c.OrderDetails)
                         .HasForeignKey(p => p.ProductId);
            });
            builder.Entity<CartItem>(entity =>
            {
                entity.HasOne(p => p.Product)
                         .WithMany(c => c.cartItems)
                         .HasForeignKey(p => p.ProductId);
            });
            builder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.User)
                         .WithMany(c => c.Payments)
                         .HasForeignKey(p => p.UserId);
            });
            builder.Entity<Order>(entity =>
            {
                entity.HasOne(p => p.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(p => p.UserId);
            });


            #region حرکت در عمق
            //builder.Entity<Order>(entity =>
            //{
            //    entity.HasOne(p => p.User)
            //             .WithMany(c => c.Orders)
            //             .HasForeignKey(p => p.UserId);
            //});


            //.OnDelete(DeleteBehavior.Cascade)
            //builder.Entity<Product>().Property(s => s.Depth).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.Height).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.Lenght).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.Thicnekss).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.Width).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.MinSize).HasDefaultValueSql("0");
            //builder.Entity<Product>().Property(s => s.MaxSize).HasDefaultValueSql("0");



            //builder.Entity<Color>(entity =>
            //{
            //    entity.Property(e => e.Id ).ValueGeneratedNever();

            //    entity.HasOne(d => d.Product)
            //        .WithOne(p => p.Color)
            //        .HasForeignKey<Color>(d => d.Id);
            //});

            //builder.Entity<MiniSize>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.HasOne(d => d.Product)
            //        .WithOne(p => p.MiniSize)
            //        .HasForeignKey<MiniSize>(d => d.Id);
            //});

            //builder.Entity<Height>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.HasOne(d => d.Product)
            //        .WithOne(p => p.Height)
            //        .HasForeignKey<Height>(d => d.Id);
            //});

            //builder.Entity<Depth>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.HasOne(d => d.Product)
            //        .WithOne(p => p.Depth)
            //        .HasForeignKey<Depth>(d => d.Id);
            //});
            //builder.Entity<Length>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.HasOne(d => d.Product)
            //        .WithOne(p => p.Length)
            //        .HasForeignKey<Length>(d => d.Id);
            //});
            #endregion
            #endregion
        }

        #region جداول
        public DbSet<GlarinWood.Models.Category> Category { get; set; }

        public DbSet<GlarinWood.Models.Product> Product { get; set; }

        public DbSet<GlarinWood.Areas.Administrator.Models.Slider> Slider { get; set; }

        public DbSet<GlarinWood.Areas.Administrator.Models.About> About { get; set; }

        public DbSet<GlarinWood.Areas.Administrator.Models.Projects> Projects { get; set; }

        public DbSet<GlarinWood.Models.HeaderColumn> HeaderColumn { get; set; }

        public DbSet<GlarinWood.Models.HeaderColumnsDes> HeaderColumnsDes { get; set; }

        public DbSet<GlarinWood.Models.Download_Files> Download_Files { get; set; }

        public DbSet<GlarinWood.Models.PicGallery> PicGallery { get; set; }

        public DbSet<GlarinWood.Models.PicCollectionGallery> PicCollectionGallery { get; set; }

        public DbSet<GlarinWood.Models.Contact> Contact { get; set; }

        public DbSet<GlarinWood.Models.CartItem> CartItems { get; set; }

        public DbSet<GlarinWood.Models.Order> Order { get; set; }

        public DbSet<GlarinWood.Models.OrderDetail> OrderDetail { get; set; }

        public DbSet<GlarinWood.Models.Payment> Payment { get; set; }
        #endregion


    }
}
