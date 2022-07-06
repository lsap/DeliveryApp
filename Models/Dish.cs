using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Models
{
    public class Dish
    {
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        public string Description{get;set;}
        public decimal Price{get;set;}
        public string ImgName{get;set;}
        public int ShopId{get;set;}
        public Shop Shop{get;set;}
    }
}