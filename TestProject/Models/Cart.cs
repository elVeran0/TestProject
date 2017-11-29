using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("carts")]
    public class Cart
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("datetime")]
        public DateTime Datetime { get; set; }
    }
}