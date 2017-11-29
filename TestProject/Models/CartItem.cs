using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    [Table("cart_items")]
    public class CartItem
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Order Order { get; set; }
    }
}