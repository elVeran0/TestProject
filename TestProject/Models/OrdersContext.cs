using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class OrdersContext : Context
    {
        public IEnumerable<Order> GetAll()
        {
            List<Order> list = new List<Order>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM 
                                                    (
	                                                    SELECT * FROM orders
	                                                    JOIN
		                                                    (SELECT air_tickets.id AS air_ticket_id, air_tickets.depart, air_tickets.arrive, air_tickets.from_city_id, air_tickets.to_city_id, air_tickets.airlines_id, air_tickets.price AS air_ticket_price
		                                                    FROM air_tickets) AS aT
	                                                    USING(air_ticket_id)
                                                    ) AS t
                                                    JOIN
	                                                    (SELECT airlines.id AS airlines_id, airlines.name AS airline_name from airlines) AS airl
                                                    USING(airlines_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS from_city_id, cities.name AS from_city_name FROM cities) AS frC
                                                    USING(from_city_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS to_city_id, cities.name AS to_city_name FROM cities) AS toC
                                                    USING(to_city_id) 
                                                    JOIN
	                                                    (SELECT hotel_orders.id AS hotel_order_id, hotel_orders.check_in, hotel_orders.check_out, hotel_orders.hotel_id, hotel_orders.price AS hotel_order_price FROM hotel_orders) AS hO
                                                    USING(hotel_order_id)
                                                    JOIN
	                                                    (SELECT hotels.id AS hotel_id, hotels.name AS hotel_name, hotels.city_id AS hotel_city_id, hotels.address, cities.name AS hotel_city_name 
	                                                     FROM hotels 
	                                                     JOIN cities ON hotels.city_id = cities.id) AS hotel
                                                    USING(hotel_id)", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Order
                            {
                                Id = Int32.Parse(reader.GetString(6)),
                                AirTicket = new AirTicket
                                {
                                    Id = reader.GetInt32(5),
                                    Depart = reader.GetDateTime(7),
                                    Arrive = reader.GetDateTime(8),
                                    FromCity = new City
                                    {
                                        Id = reader.GetInt32(3),
                                        Name = reader.GetString(11)
                                    },
                                    ToCity = new City
                                    {
                                        Id = reader.GetInt32(2),
                                        Name = reader.GetString(12)
                                    },
                                    Airline = new Airline
                                    {
                                        Id = reader.GetInt32(4),
                                        Name = reader.GetString(10)
                                    },
                                    Price = reader.GetDecimal(9)
                                },
                                HotelOrder = new HotelOrder
                                {
                                    Id = reader.GetInt32(1),
                                    CheckIn = reader.GetDateTime(13),
                                    CheckOut = reader.GetDateTime(14),
                                    Hotel = new Hotel
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(16),
                                        City = new City
                                        {
                                            Id = reader.GetInt32(17),
                                            Name = reader.GetString(19)
                                        },
                                        Address = reader.GetString(18)
                                    },
                                    Price = reader.GetDecimal(15)
                                }
                            }
                                    );
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public Order Get(int id)
        {
            Order order = new Order
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                
                using (var cmd = new NpgsqlCommand(@"SELECT * FROM 
                                                    (
	                                                    (SELECT * FROM orders WHERE orders.id = @id) AS ord
	                                                    JOIN
		                                                    (SELECT air_tickets.id AS air_ticket_id, air_tickets.depart, air_tickets.arrive, air_tickets.from_city_id, air_tickets.to_city_id, air_tickets.airlines_id, air_tickets.price AS air_ticket_price
		                                                    FROM air_tickets) AS aT
	                                                    USING(air_ticket_id)
                                                    ) AS t
                                                    JOIN
	                                                    (SELECT airlines.id AS airlines_id, airlines.name AS airline_name FROM airlines) AS airl
                                                    USING(airlines_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS from_city_id, cities.name AS from_city_name FROM cities) AS frC
                                                    USING(from_city_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS to_city_id, cities.name AS to_city_name FROM cities) AS toC
                                                    USING(to_city_id) 
                                                    JOIN
	                                                    (SELECT hotel_orders.id AS hotel_order_id, hotel_orders.check_in, hotel_orders.check_out, hotel_orders.hotel_id, hotel_orders.price AS hotel_order_price
		                                                       FROM hotel_orders) AS hO
                                                    USING (hotel_order_id)
                                                    JOIN
	                                                    (SELECT hotels.id AS hotel_id, hotels.name AS hotel_name, hotels.city_id AS hotel_city_id, hotels.address, cities.name AS hotel_city_name 
	                                                     FROM hotels 
	                                                     JOIN cities ON hotels.city_id = cities.id) AS hotel
                                                    USING(hotel_id)", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order.AirTicket = new AirTicket
                            {
                                Id = reader.GetInt32(5),
                                Depart = reader.GetDateTime(7),
                                Arrive = reader.GetDateTime(8),
                                FromCity = new City
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(11)
                                },
                                ToCity = new City
                                {
                                    Id = reader.GetInt32(2),
                                    Name = reader.GetString(12)
                                },
                                Airline = new Airline
                                {
                                    Id = reader.GetInt32(4),
                                    Name = reader.GetString(10)
                                },
                                Price = reader.GetDecimal(9)
                            };

                            order.HotelOrder = new HotelOrder
                            {
                                Id = reader.GetInt32(1),
                                CheckIn = reader.GetDateTime(13),
                                CheckOut = reader.GetDateTime(14),
                                Hotel = new Hotel
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(16),
                                    City = new City
                                    {
                                        Id = reader.GetInt32(17),
                                        Name = reader.GetString(19)
                                    },
                                    Address = reader.GetString(18)
                                },
                                Price = reader.GetDecimal(15)
                            };
                        }
                    }
                }

                connection.Close();
            }

            return order;
        }

        public int Add(Order order)
        {
            int id = 0;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO orders (air_ticket_id, hotel_order_id) VALUES (@atId, @hoId) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("atId", order.AirTicket.Id);
                        cmd.Parameters.AddWithValue("hoId", order.HotelOrder.Id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                id = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception)
                {
                    id = 0;
                }

                connection.Close();
            }

            return id;
        }

        public void Update(Order order)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE orders SET (air_ticket_id, hotel_order_id) = (@atId, @hoId) WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", order.Id);
                    cmd.Parameters.AddWithValue("atId", order.AirTicket.Id);
                    cmd.Parameters.AddWithValue("hoId", order.HotelOrder.Id);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            Order deleteItem = Get(id);

            if (deleteItem == null || deleteItem.HotelOrder == null || deleteItem.AirTicket == null)
                return;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM orders WHERE Id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    connection.Close();
                    return;
                }

                connection.Close();
            }

            HotelOrdersContext hoContext = new HotelOrdersContext();
            hoContext.Delete(deleteItem.HotelOrder.Id);
            AirTicketsContext atContext = new AirTicketsContext();
            atContext.Delete(deleteItem.AirTicket.Id);
        }
    }
}