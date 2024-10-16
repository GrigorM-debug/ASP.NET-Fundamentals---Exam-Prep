using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Models;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using static SoftUniBazar.Constants.ApplicationConstants;
using System.Security.Claims;
using NuGet.Common;

namespace SoftUniBazar.Controllers
{
    public class AdController : Controller
    {
        private readonly BazarDbContext _context;

        public AdController(BazarDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<AdIndexViewModel> adsModels = await _context
                .Ads
                .AsNoTracking()
                .Select(ad => new AdIndexViewModel()
                {
                    Id = ad.Id,
                    CreatedOn = ad.CreatedOn.ToString(DateTimeFormat),
                    Description = ad.Description,
                    ImageUrl = ad.ImageUrl,
                    Name = ad.Name,
                    Owner = ad.Owner.UserName,
                    Category = ad.Category.Name,
                    Price = ad.Price.ToString("F2")
                }).ToListAsync();

            return View(adsModels);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddInputViewModel model = new AddInputViewModel();
            model.Categories = await GetCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddInputViewModel inputModel)
        {
            if (ModelState.IsValid == false)
            {
                inputModel.Categories = await GetCategories();
                return View(inputModel);
            }

            Ad newAd = new Ad()
            {
                Name = inputModel.Name,
                CategoryId = inputModel.CategoryId,
                CreatedOn = DateTime.Now,
                Description = inputModel.Description,
                OwnerId = GetUserId(),
                ImageUrl = inputModel.ImageUrl,
                Price = inputModel.Price,
            };

            await _context.Ads.AddAsync(newAd);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = GetUserId();

            Ad? ad = await _context
                .Ads
                .FirstOrDefaultAsync(a => a.Id == id && a.OwnerId == userId);

            if (ad == null)
            {
                return RedirectToAction(nameof(All));
            }

            AddInputViewModel model = new AddInputViewModel
            {
                Name = ad.Name,
                Description = ad.Description,
                ImageUrl = ad.ImageUrl,
                Price = ad.Price,
                CategoryId = ad.CategoryId,
                Categories = await GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddInputViewModel inputModel, int id)
        {
            string userId = GetUserId();

            Ad? ad = await _context
                .Ads
                .FirstOrDefaultAsync(a => a.Id == id && a.OwnerId == userId);

            if (ad == null)
            {
                return RedirectToAction(nameof(All));
            }

            ad.Name = inputModel.Name;
            ad.Description = inputModel.Description;
            ad.ImageUrl = inputModel.ImageUrl;
            ad.Price = inputModel.Price;
            ad.CategoryId = inputModel.CategoryId;
            ad.CreatedOn = DateTime.Now;
            ad.OwnerId = GetUserId();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string userId = GetUserId();

            IEnumerable<AdIndexViewModel> models = await _context
                .AdsBuyers
                .AsNoTracking()
                .Where(ab => ab.BuyerId == userId)
                .Include(ab => ab.Ad)
                .ThenInclude(a => a.Category)
                .Include(ab => ab.Buyer)
                .Select(ab => new AdIndexViewModel
                {
                    Id = ab.AdId,
                    Name = ab.Ad.Name,
                    ImageUrl = ab.Ad.ImageUrl,
                    CreatedOn = ab.Ad.CreatedOn.ToString(DateTimeFormat),
                    Description = ab.Ad.Description,
                    Price = ab.Ad.Price.ToString("F2"),
                    Category = ab.Ad.Category.Name,
                    Owner = ab.Ad.Owner.UserName,
                }).ToListAsync();

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            //Checking if the Ad is existing in the db
            Ad? ad = await _context
                .Ads.FindAsync(id);

            if (ad == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Checking if the Ad is already added in the user Cart
            string userId = GetUserId();

            AdBuyer? adBuyer = await _context
                .AdsBuyers
                .FirstOrDefaultAsync(ab => ab.AdId == id && ab.BuyerId == userId);

            if (adBuyer != null)
            {
                return RedirectToAction(nameof(All));
            }

            //Adding Ad to User Collection
            await _context.AdsBuyers.AddAsync(new AdBuyer()
            {
                AdId = id,
                BuyerId = userId
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            //Checking if the Ad is existing in the db
            Ad? ad = await _context
                .Ads.FindAsync(id);

            if (ad == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Checking if the Ad is already added in the user Cart
            string userId = GetUserId();

            AdBuyer? adBuyer = await _context
                .AdsBuyers
                .FirstOrDefaultAsync(ab => ab.AdId == id && ab.BuyerId == userId);

            if (adBuyer == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Removing Ad from User Collection
            _context.AdsBuyers.Remove(adBuyer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<CategorySelectList>> GetCategories()
        {
            IEnumerable<CategorySelectList> categories = await _context
                .Categories
                .AsNoTracking()
                .Select(c => new CategorySelectList()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();

            return categories;
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }
    }
}
