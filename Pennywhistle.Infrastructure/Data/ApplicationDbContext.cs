using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Domain.Common;
using Pennywhistle.Domain.Entities;
using Pennywhistle.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Infrastructure.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
       

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
          
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.SeedRoles(builder);
            this.SeedUsers(builder);
            this.SeedUserRoles(builder);

           // builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        #region Seed Data
        private void SeedRoles(ModelBuilder builder)
        {


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                 new IdentityRole() { Id = "aab4fac1-c546-41de-aebc-a14da6895712", Name = "StoreStaff", ConcurrencyStamp = "1", NormalizedName = "STORESTAFF" },
                  new IdentityRole() { Id = "bab4fac1-c546-41de-aebc-a14da6895713", Name = "KitchenStaff", ConcurrencyStamp = "1", NormalizedName = "KITCHENSTAFF" },
                   new IdentityRole() { Id = "cab4fac1-c546-41de-aebc-a14da6895714", Name = "DeliveryStaff", ConcurrencyStamp = "1", NormalizedName = "DELIVERYSTAFF" },
                   new IdentityRole() { Id = "dab4fac1-c546-41de-aebc-a14da6895715", Name = "Customer", ConcurrencyStamp = "1", NormalizedName = "CUSTOMER" }
                );
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            ApplicationUser user = new ApplicationUser()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "adminuser",
                NormalizedUserName = "ADMINUSER",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                PasswordHash = hasher.HashPassword(null, "1234")
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            passwordHasher.HashPassword(user, "1234");

            builder.Entity<ApplicationUser>().HasData(user);
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
                );
        }
        #endregion
    }
}
