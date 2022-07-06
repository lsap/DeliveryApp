using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Models
{
    public class Order
    {
        public int Id{get;set;}
        [Required]
        public string CustomerName{get;set;}
        [Required]
        public string Email{get;set;}
        [Required]
        public string Phone{get;set;}
        [Required]
        public string Address{get;set;}
        public IEnumerable<OrderedDish> OrderedDishes{get;set;}
    }
}