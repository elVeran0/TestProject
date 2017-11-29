using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("hotel_orders")]
    public class HotelOrder
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("check_in")]
        public DateTime CheckIn { get; set; }

        [Column("check_out")]
        public DateTime CheckOut { get; set; }

        [Column("hotel_id")]
        public int HotelId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
        
        public virtual Hotel Hotel{ get; set; }
    }
}

