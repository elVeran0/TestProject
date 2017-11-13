using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; } // FK user(id)
        public DateTime Datetime { get; set; }
    }
}