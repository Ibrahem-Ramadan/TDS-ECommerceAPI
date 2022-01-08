using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {}


        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<UsersProducts> UsersProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");

            builder.Entity<IdentityRole>().ToTable("Roles");

            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");

            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");

            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");

            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");

            builder.Entity<UsersProducts>().HasKey(u => new { u.ProductId, u.UserId , u.OrderId});

        }

    }

}
