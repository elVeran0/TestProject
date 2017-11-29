using System;
using Npgsql;
using System.Data.Entity;

namespace TestProject.Models
{
    public class Context : DbContext
    {
        public DbSet<City> cities { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<Airline> airlines { get; set; }
        public DbSet<AirTicket> airTickets { get; set; }
        public DbSet<AirTicketBase> airTicketsBase { get; set; }
        public DbSet<HotelOrder> hotelOrders { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Order> orders { get; set; }

        private const string bConnectionString = "Host=localhost;Username=postgres;Password=1234;Database=test_project_db";
        
        public Context(string connectionString = bConnectionString) : base(connectionString)
        { }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //default Visual Studio schema - dto, Postgresql - public
            modelBuilder.HasDefaultSchema("public");
        }
    }
    
}