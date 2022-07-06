using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public string CustomerName{get;set;}
        [Required]
        public string Email{get;set;}
        [Required]
        public string Phone{get;set;}
        [Required]
        public string Address{get;set;}
        public Dictionary<int, int> OrderedDishes{get;set;}
    }
}