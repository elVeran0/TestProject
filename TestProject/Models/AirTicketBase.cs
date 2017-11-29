using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("air_tickets_base")]
    public class AirTicketBase
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("depart")]
        public DateTime Depart { get; set; }

        [Column("arrive")]
        public DateTime Arrive { get; set; }

        [Column("from_city_id")]
        public int FromCityId { get; set; }

        [Column("to_city_id")]
        public int ToCityId { get; set; }
        
        [Column("airlines_id")]
        public int AirlineId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        public virtual City FromCity { get; set; }
        public virtual City ToCity { get; set; }
        public virtual Airline Airline { get; set; }


        public static implicit operator AirTicket(AirTicketBase atBase)
        {
            AirTicket at = new AirTicket
            {
                Depart = atBase.Depart,
                Arrive = atBase.Arrive,
                FromCityId = atBase.FromCityId,
                ToCityId = atBase.ToCityId,
                AirlineId = atBase.AirlineId,
                Price = atBase.Price,
                FromCity = atBase.FromCity,
                ToCity = atBase.ToCity,
                Airline = atBase.Airline
            };

            return at;
        }
    }
}

