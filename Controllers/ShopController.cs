using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DeliveryApp.ViewModels;

namespace DeliveryApp.Controllers
{
    public class ShopController : Controller
    {
        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext _dbContext;
        public async Task<IActionResult> Index()
        {
            return View(new ShopListViewModel{Shops = await _dbContext.Shops.ToListAsync()});
        }
        public async Task<IActionResult> GetDishes(int? shopId)
        {
            if(shopId == null)return BadRequest("Shop hasn't been specified.");
            return new ObjectResult(await _dbContext.Dishes.Where(di => di.ShopId == shopId).ToListAsync());
        }

        public IActionResult ShopLogo(string imgName)
        {
            return GetImage("ImageStore/Logos", imgName);
        }
        public IActionResult Photo(string imgName)
        {
            return GetImage("ImageStore/Photos", imgName);
        }
        private IActionResult GetImage(string path, string imgName)
        {
            if(imgName == null)return BadRequest($"'{imgName}' isn't specified.");
            var logo = System.IO.File.OpenRead(Path.Combine(path, imgName));
            if(logo == null)return BadRequest($"Logo '{imgName}' haven't been found.");
            return File(logo, "image/jpeg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
