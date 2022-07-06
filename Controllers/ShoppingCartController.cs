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
    public class ShoppingCartController : Controller
    {
        public ShoppingCartController(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext _dbContext;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetDish(int? dishId)
        {
            if(dishId == null)return BadRequest("Ids haven't been specified.");
            return new ObjectResult(await _dbContext.Dishes
                                                    .Where(di => di.Id == dishId)
                                                    .FirstAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Order(OrderViewModel order)
        {
            if(ModelState.IsValid)
            {
                Order orderInDb = new Order{
                    CustomerName = order.CustomerName,
                    Email = order.Email,
                    Phone = order.Phone,
                    Address = order.Address
                };
                _dbContext.Orders.Add(orderInDb);
                await _dbContext.SaveChangesAsync();
                foreach(var item in order.OrderedDishes)
                {
                    _dbContext.OrderedDishes.Add(new OrderedDish{
                        OrderId = orderInDb.Id,
                        DishId = item.Key,
                        Quantity = item.Value
                    });
                }
                await _dbContext.SaveChangesAsync();
            }
            return View("Index", order);
        }
    }
}