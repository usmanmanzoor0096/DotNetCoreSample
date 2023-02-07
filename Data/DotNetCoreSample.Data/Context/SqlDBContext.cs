using AuthService.Common.Models.DBModels;
using AuthService.Data.Seed;
using AuthService.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthService.Data.Context
{
    public class SqlDBContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        //public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public virtual DbSet<RefreshToken> RefreshToken { get; set; } 
        public virtual DbSet<OAuthToken> OAuthTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedData();
        }
        public SqlDBContext(DbContextOptions<SqlDBContext> options) 
            : base(options)
        {
        }
    }
}
