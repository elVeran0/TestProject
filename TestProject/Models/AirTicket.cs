﻿using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("air_tickets")]
    public class AirTicket
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


        public static implicit operator AirTicketBase(AirTicket at)
        {
            AirTicketBase atBase = new AirTicketBase
            {
                Depart = at.Depart,
                Arrive = at.Arrive,
                FromCityId = at.FromCityId,
                ToCityId = at.ToCityId,
                AirlineId = at.AirlineId,
                Price = at.Price,
                FromCity = at.FromCity,
                ToCity = at.ToCity,
                Airline = at.Airline
            };

            return atBase;
        }
    }
}

