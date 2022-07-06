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
using DeliveryApp.Models;

namespace DeliveryApp.Controllers
{
    public class AdminController : Controller
    {
        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext _dbContext;

        public async Task<IActionResult> Orders()
        {
            return View(await _dbContext.Orders.Include(or=>or.OrderedDishes)
                                               .ThenInclude(od => od.Dish)
                                               .ToListAsync());
        }
    }
}