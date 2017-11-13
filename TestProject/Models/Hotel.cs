using System;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public City City { get; set; }
        public String Address{ get; set; }
    }
}