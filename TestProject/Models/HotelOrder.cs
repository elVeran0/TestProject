using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class HotelOrder
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Hotel Hotel{ get; set; }
        public Decimal Price { get; set; }
    }
}

