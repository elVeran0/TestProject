using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;


namespace TestProject.Models
{
    [Table("hotels")]
    public class Hotel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("city_id")]
        public int CityId { get; set; }

        [Column("address")]
        public string Address{ get; set; }
        
        public virtual City City { get; set; }
    }
}