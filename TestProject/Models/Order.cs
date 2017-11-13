using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public AirTicket AirTicket { get; set; }
        public HotelOrder HotelOrder { get; set; }
    }
}

