using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shoppu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppu.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitializer(
            ILogger<ApplicationDbContextInitializer> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on db initialization.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on db seeding.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            //default roles
            var adminRole = new IdentityRole("Admin");
            if (!_roleManager.Roles.Any(r => r.Name == adminRole.Name))
            {
                await _roleManager.CreateAsync(adminRole);
            }

            //default users

            var admin = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

            if (!_userManager.Users.Any(u => u.UserName == admin.UserName))
            {
                await _userManager.CreateAsync(admin, "!Q1w2e3r4");
                await _userManager.AddToRoleAsync(admin, adminRole.Name);
            }

            //default data

            if (!_context.Variants.Any())
            {
                string[] variants = { "White", "Red", "Blue", "Black" };
                foreach (var variant in variants)
                {
                    _context.Variants.Add(new Variant
                    {
                        Name = variant
                    });
                }
            }

            if (!_context.Variants.Any())
            {
                string[] sizes = { "S", "M", "L", "XL" };
                foreach (var size in sizes)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size
                    });
                }
            }

            if (!_context.ProductTypes.Any())
            {
                var shoe = _context.ProductTypes.Add(new ProductType
                {
                    Name = "Shoe",
                });

            }

            await _context.SaveChangesAsync();

            if (!_context.Products.Any())
            {
                var shoe = _context.ProductTypes.FirstOrDefault(p => p.Name == "Shoe");
                _context.Products.Add(new Product
                {
                    Name = "Yeezes",
                    Description = "Nice shoes",
                    Price = 39.99m,
                    ProductTypeId = shoe.Id,
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
