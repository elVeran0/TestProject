using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Cart Cart { get; set; }
        public Order Order { get; set; }
    }
}