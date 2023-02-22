using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Extensions;
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
                    Name = "Clothes",
                    UrlName = "clothes",
                });

                var accessories = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Underwear and accessories",
                    UrlName = "accessories",
                });

                await _context.SaveChangesAsync();


                var tShirts = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "T-Shirts",
                    UrlName = "tshirts",
                    ParentCategoryId = clothes.Entity.Id,

                });
                var pants = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Pants",
                    UrlName = "pants",
                    ParentCategoryId = clothes.Entity.Id,
                });

                var shoes = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Shoes",
                    UrlName = "shoes",
                    ParentCategoryId = accessories.Entity.Id,
                });
                var socks = _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Socks",
                    UrlName = "socks",
                    ParentCategoryId = accessories.Entity.Id,
                });

                await _context.SaveChangesAsync();

                _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Shorts",
                    UrlName = "shorts",
                    ParentCategoryId = pants.Entity.Id,
                });
                _context.ProductCategories.Add(new ProductCategory
                {
                    Name = "Joggers",
                    UrlName = "joggers",
                    ParentCategoryId = pants.Entity.Id,
                });
            }

            await _context.SaveChangesAsync();

            if (!_context.Variants.Any())
            {
                (string, string)[] variants = new (string, string)[]
                    {
                        ("White", "#DDE4F1"),
                        ("Red", "#FF4C4C"),
                        ("Blue", "#3232FF"),
                        ("Black", "#000000"),
                        ("Green", "#66B266"),
                        ("Orange", "#FFC966"),
                        ("Yellow", "#FFFF99"),
                        ("Purple", "#8C198C"),
                        ("Beige", "#fff0db"),
                    };
                foreach (var variant in variants)
                {
                    _context.Variants.Add(new Variant
                    {
                        Name = variant.Item1,
                        HEXColor = variant.Item2
                    });
                }
            }

            if (!_context.Sizes.Any())
            {
                string[] sizesLetters = { "XS", "S", "M", "L", "XL", "XXL" };
                string[] sizesPants = { "34", "36", "38", "40", "42", "44" };

                _context.SizeTypes.Add(new SizeType
                {
                    Name = "Letters",
                });

                _context.SizeTypes.Add(new SizeType
                {
                    Name = "Numbers",
                });

                _context.SizeTypes.Add(new SizeType
                {
                    Name = "Socks",
                });

                await _context.SaveChangesAsync();

                var sizeTypeLetters = _context.SizeTypes.FirstOrDefault(st => st.Name == "Letters");
                foreach (var size in sizesLetters)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        SizeTypeId = sizeTypeLetters.Id
                    });
                }

                var sizeTypeNumbers = _context.SizeTypes.FirstOrDefault(st => st.Name == "Numbers");

                for (int i = 30; i < 50; i++)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = i.ToString(),
                        SizeTypeId = sizeTypeNumbers.Id
                    });
                }

                var sizeTypeSocks = _context.SizeTypes.FirstOrDefault(st => st.Name == "Numbers");

                string[] sizesSocks = { "35-38", "39-42", "43-46" };

                foreach (var size in sizesSocks)
                {
                    _context.Sizes.Add(new Size
                    {
                        Name = size,
                        SizeTypeId = sizeTypeSocks.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
