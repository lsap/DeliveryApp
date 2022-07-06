using System.Collections.Generic;
using DeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            if (Database.EnsureCreated())
            {
                Init();
            }
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedDish> OrderedDishes { get; set; }


        private void Init()
        {
            var shops = new List<Shop>{
                new Shop{Name="Mc Donny", Description="Mc Donny description.", ImgName="1.jfif"},
                new Shop{Name="My food", Description="My food description.", ImgName="2.jfif"},
                new Shop{Name="Delivery GO", Description="Delivery GO description.", ImgName="3.jfif"},
                new Shop{Name="Eater", Description="Eater description.", ImgName="4.jfif"},
                new Shop{Name="Hi! Pizza", Description="Hi! Pizza description.", ImgName="5.jfif"},
                new Shop{Name="THY", Description="THY description.", ImgName="6.jfif"},
            };
            Shops.AddRange(shops);
            SaveChanges();
            Dishes.AddRange(new List<Dish>{
                    new Dish{Name="Pizza 1", Description="Pizza 1 description", Price=1.6m ,ImgName="1.webp", ShopId=shops[0].Id},
                    new Dish{Name="Pizza 2", Description="Pizza 2 description", Price=2.9m ,ImgName="2.jpg", ShopId=shops[0].Id},
                    new Dish{Name="Burger", Description="Burger description", Price=1.2m ,ImgName="3.jpg", ShopId=shops[0].Id},
                    new Dish{Name="Mini Sendwich", Description="Mini Sendwich description", Price=0.65m ,ImgName="4.jpg", ShopId=shops[0].Id}
                });
            Dishes.AddRange(new List<Dish>{
                    new Dish{Name="Salad 1", Description="Salad 1 description", Price=1.6m ,ImgName="5.webp", ShopId=shops[1].Id},
                    new Dish{Name="Salad 2", Description="Salad 2 description", Price=2.9m ,ImgName="6.jpg", ShopId=shops[1].Id}
                });
            Dishes.AddRange(new List<Dish>{
                    new Dish{Name="Борщ", Description="Найсмачніший борщ тільки у нас", Price=10m ,ImgName="7.webp", ShopId=shops[2].Id},
                    new Dish{Name="Галушки", Description="Галушки - наше все", Price=11m ,ImgName="8.jpg", ShopId=shops[2].Id},
                    new Dish{Name="Вареники", Description="Домашні по-українськи", Price=15m ,ImgName="9.jpg", ShopId=shops[2].Id}
                });
            SaveChanges();
        }
    }
}