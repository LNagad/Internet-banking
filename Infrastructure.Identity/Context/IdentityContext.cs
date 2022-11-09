using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Context
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //FLUENT API

            base.OnModelCreating(builder);

            #region "tables"

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(p =>
            {
                p.ToTable("Users");
            });

            builder.Entity<IdentityRole>(p =>
            {
                p.ToTable("Roles");
            });

            builder.Entity<IdentityUserRole<string>>(p =>
            {
                p.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserLogin<string>>(p =>
            {
                p.ToTable("UserLogins");
            });


            #endregion
        }
    }
}
