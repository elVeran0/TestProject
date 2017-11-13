using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class AirTicket
    {
        public int Id { get; set; }
        public DateTime Depart { get; set; }
        public DateTime Arrive { get; set; }
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public Airline Airline { get; set; }
        public decimal Price { get; set; }
    }
}

