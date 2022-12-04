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


            if (!_context.ProductCategories.Any())
            {
                var clothes = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Clothes"
                });

                var accessories = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Underwear and accessiories"
                });

                await _context.SaveChangesAsync();


                var tShirts = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "T-Shirts",
                    ParentCategoryId = clothes.Entity.Id,
                    
                });
                var pants = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Pants",
                    ParentCategoryId = clothes.Entity.Id,
                });

                var shoes = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Shoes",
                    ParentCategoryId = accessories.Entity.Id,
                });
                var socks = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Socks",
                    ParentCategoryId = accessories.Entity.Id,
                });

                await _context.SaveChangesAsync();

                _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Shorts",
                    ParentCategoryId = pants.Entity.Id,
                });
                _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Jogger",
                    ParentCategoryId = pants.Entity.Id,
                });
            }

            await _context.SaveChangesAsync();

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

            if (!_context.Sizes.Any())
            {
                string[] sizesShirts = { "S", "M", "L", "XL" };
                string[] sizesPants = { "38", "40", "42", "44" };
                var tShirts = _context.ProductCategories.FirstOrDefault(p => p.Name == "T-Shirts");
                foreach (var size in sizesShirts)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        ProductCategoryId = tShirts.Id
                    });
                }

                var shorts = _context.ProductCategories.FirstOrDefault(p => p.Name == "Shorts");
                var joggers = _context.ProductCategories.FirstOrDefault(p => p.Name == "Jogger");
                foreach (var size in sizesPants)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        ProductCategoryId = shorts.Id
                    });
                }
                foreach (var size in sizesPants)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        ProductCategoryId = joggers.Id
                    });
                }

                string[] sizesShoes = { "36", "37", "38", "39", "40", "41", "42", "43" };
                string[] sizesSocks = { "35-38", "39-42", "43-46" };
                var shoes = _context.ProductCategories.FirstOrDefault(p => p.Name == "Shoes");
                var socks = _context.ProductCategories.FirstOrDefault(p => p.Name == "Socks");
                foreach (var size in sizesShoes)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        ProductCategoryId = shoes.Id
                    });
                }
                foreach (var size in sizesSocks)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        ProductCategoryId = socks.Id
                    });
                }
            }

            if (!_context.Products.Any())
            {
                var tShirts = _context.ProductCategories.FirstOrDefault(p => p.Name == "T-Shirts");
                var shorts = _context.ProductCategories.FirstOrDefault(p => p.Name == "Shorts");
                var joggers = _context.ProductCategories.FirstOrDefault(p => p.Name == "Jogger");

                var shoes = _context.ProductCategories.FirstOrDefault(p => p.Name == "Shoes");
                var socks = _context.ProductCategories.FirstOrDefault(p => p.Name == "Socks");

                _context.Products.Add(new Product
                {
                    Name = "Puma",
                    Description = "Puma shirt",
                    Price = 34.99m,
                    ProductCategoryId = tShirts.Id,
                });
                _context.Products.Add(new Product
                {
                    Name = "Gucci shirto",
                    Description = "Its all gucci",
                    Price = 74.99m,
                    ProductCategoryId = tShirts.Id,
                });

                _context.Products.Add(new Product
                {
                    Name = "Shorty",
                    Description = "Shorty pants",
                    Price = 19.99m,
                    ProductCategoryId = shorts.Id,
                });
                _context.Products.Add(new Product
                {
                    Name = "Joggu",
                    Description = "Joggy pants",
                    Price = 54.99m,
                    ProductCategoryId = joggers.Id,
                });

                _context.Products.Add(new Product
                {
                    Name = "Yeezes",
                    Description = "Nice shoes",
                    Price = 39.99m,
                    ProductCategoryId = shoes.Id,
                });
                _context.Products.Add(new Product
                {
                    Name = "Vans",
                    Description = "Vans shoes",
                    Price = 24.99m,
                    ProductCategoryId = shoes.Id,
                });
                _context.Products.Add(new Product
                {
                    Name = "Socky",
                    Description = "Socks 3-pack",
                    Price = 14.99m,
                    ProductCategoryId = socks.Id,
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
