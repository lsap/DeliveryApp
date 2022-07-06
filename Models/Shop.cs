using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Models
{
    public class Shop
    {
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        public string Description{get;set;}
        [Required]
        public string ImgName{get;set;}
        public IEnumerable<Dish> Dishes{get;set;}
    }
}