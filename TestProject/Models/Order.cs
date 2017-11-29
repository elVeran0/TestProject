using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("orders")]
    public class Order
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("air_ticket_id")]
        public int AirTicketId { get; set; }

        [Column("hotel_order_id")]
        public int HotelOrderId { get; set; }
        
        public virtual AirTicket AirTicket { get; set; }
        public virtual HotelOrder HotelOrder { get; set; }
    }
}

