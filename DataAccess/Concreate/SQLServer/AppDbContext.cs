using Entities.Concreate;
using Entities.DTO.ProductDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concreate.SQLServer
{
    public class AppDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = LOCALHOST\\SQLEXPRESS;  DataBase = KarlEcommerceApp3; Trusted_Connection = True; MultipleActiveResultSets = True;  TrustServerCertificate = true;");
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLanguage> ProductLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");

            builder.Entity<ProductAdminListDTO>().ToView(null);

             builder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreateDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdateDate = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        data.Entity.DeleteDate = DateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
