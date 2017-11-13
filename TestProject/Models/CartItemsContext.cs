using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class CartItemsContext : Context
    {
        public IEnumerable<CartItem> GetAll()
        {
            List<CartItem> list = new List<CartItem>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM cart_items
                                                    JOIN
                                                    (
	                                                    SELECT carts.id AS cart_id, carts.user_id, carts.datetime AS cart_datetime FROM carts
                                                    ) AS cart
                                                    USING(cart_id)
                                                    JOIN
                                                    (
	                                                    SELECT orders.id AS order_id, orders.hotel_order_id, at.* FROM orders
	                                                    JOIN
		                                                    (SELECT air_tickets.id AS air_ticket_id, air_tickets.depart, air_tickets.arrive, air_tickets.from_city_id, air_tickets.to_city_id, air_tickets.airlines_id, air_tickets.price AS air_ticket_price
		                                                    FROM air_tickets) AS aT
	                                                    USING(air_ticket_id)
                                                    ) AS t
                                                    USING(order_id)
                                                    JOIN
	                                                    (SELECT airlines.id AS airlines_id, airlines.name AS airline_name FROM airlines) AS airl
                                                    USING(airlines_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS from_city_id, cities.name AS from_city_name FROM cities) AS frC
                                                    USING(from_city_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS to_city_id, cities.name AS to_city_name from cities) AS toC
                                                    USING(to_city_id) 
                                                    JOIN
	                                                    (SELECT hotel_orders.id AS hotel_order_id, hotel_orders.check_in, hotel_orders.check_out, hotel_orders.hotel_id, hotel_orders.price AS hotel_order_price
		                                                       FROM hotel_orders) AS hO
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
                            list.Add(new CartItem
                            {
                                Id = reader.GetInt32(7),
                                Cart = new Cart
                                {
                                    Id = reader.GetInt32(6),
                                    UserId = reader.GetInt32(8),
                                    Datetime = reader.GetDateTime(9)
                                },
                                Order = new Order
                                {
                                    Id = reader.GetInt32(5),
                                    AirTicket = new AirTicket
                                    {
                                        Id = reader.GetInt32(10),
                                        Depart = reader.GetDateTime(11),
                                        Arrive = reader.GetDateTime(12),
                                        FromCity = new City
                                        {
                                            Id = reader.GetInt32(3),
                                            Name = reader.GetString(15)
                                        },
                                        ToCity = new City
                                        {
                                            Id = reader.GetInt32(2),
                                            Name = reader.GetString(16)
                                        },
                                        Airline = new Airline
                                        {
                                            Id = reader.GetInt32(4),
                                            Name = reader.GetString(14)
                                        },
                                        Price = reader.GetDecimal(13)
                                    },
                                    HotelOrder = new HotelOrder
                                    {
                                        Id = reader.GetInt32(1),
                                        CheckIn = reader.GetDateTime(17),
                                        CheckOut = reader.GetDateTime(18),
                                        Hotel = new Hotel
                                        {
                                            Id = reader.GetInt32(0),
                                            Name = reader.GetString(20),
                                            City = new City
                                            {
                                                Id = reader.GetInt32(21),
                                                Name = reader.GetString(23)
                                            },
                                            Address = reader.GetString(22)
                                        },
                                        Price = reader.GetDecimal(19)
                                    }
                                }
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public CartItem Get(int id)
        {
            CartItem cartItem = new CartItem
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM
                                                    (SELECT * FROM cart_items WHERE cart_items.id = @id) AS tt
                                                    JOIN
                                                    (
	                                                    SELECT carts.id AS cart_id, carts.user_id, carts.datetime AS cart_datetime FROM carts
                                                    ) AS cart
                                                    USING(cart_id)
                                                    JOIN
                                                    (
	                                                    SELECT orders.id AS order_id, orders.hotel_order_id, at.* FROM orders
	                                                    JOIN
		                                                    (SELECT air_tickets.id AS air_ticket_id, air_tickets.depart, air_tickets.arrive, air_tickets.from_city_id, air_tickets.to_city_id, air_tickets.airlines_id, air_tickets.price AS air_ticket_price
		                                                    FROM air_tickets) AS aT
	                                                    USING(air_ticket_id)
                                                    ) AS t
                                                    USING(order_id)
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
	                                                    (SELECT hotel_orders.id AS hotel_order_id, hotel_orders.check_in, hotel_orders.check_out, hotel_orders.hotel_id, hotel_orders.price AS hotel_order_price FROM hotel_orders) AS hO
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
                            cartItem.Cart = new Cart
                            {
                                Id = reader.GetInt32(6),
                                UserId = reader.GetInt32(8),
                                Datetime = reader.GetDateTime(9)
                            };
                            cartItem.Order = new Order
                            {
                                Id = reader.GetInt32(5),
                                AirTicket = new AirTicket
                                {
                                    Id = reader.GetInt32(10),
                                    Depart = reader.GetDateTime(11),
                                    Arrive = reader.GetDateTime(12),
                                    FromCity = new City
                                    {
                                        Id = reader.GetInt32(3),
                                        Name = reader.GetString(15)
                                    },
                                    ToCity = new City
                                    {
                                        Id = reader.GetInt32(2),
                                        Name = reader.GetString(16)
                                    },
                                    Airline = new Airline
                                    {
                                        Id = reader.GetInt32(4),
                                        Name = reader.GetString(14)
                                    },
                                    Price = reader.GetDecimal(13)
                                },
                                HotelOrder = new HotelOrder
                                {
                                    Id = reader.GetInt32(1),
                                    CheckIn = reader.GetDateTime(17),
                                    CheckOut = reader.GetDateTime(18),
                                    Hotel = new Hotel
                                    {
                                       Id = reader.GetInt32(0),
                                       Name = reader.GetString(20),
                                       City = new City
                                       {
                                           Id = reader.GetInt32(21),
                                           Name = reader.GetString(23)
                                       },
                                       Address = reader.GetString(22)
                                    },
                                    Price = reader.GetDecimal(19)
                                }
                            };
                        }
                    }
                }

                connection.Close();
            }

            return cartItem;
        }
        
        public IEnumerable<CartItem> Get(CartItem searchCart)
        {
            List<CartItem> list = new List<CartItem>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM cart_items
                                                    JOIN
                                                    (
	                                                    SELECT carts.id AS cart_id, carts.user_id, carts.datetime AS cart_datetime FROM carts WHERE carts.id = @cId
                                                    ) AS cart
                                                    USING(cart_id)
                                                    JOIN
                                                    (
	                                                    SELECT orders.id AS order_id, orders.hotel_order_id, at.* FROM orders
	                                                    JOIN
		                                                    (SELECT air_tickets.id AS air_ticket_id, air_tickets.depart, air_tickets.arrive, air_tickets.from_city_id, air_tickets.to_city_id, air_tickets.airlines_id, air_tickets.price AS air_ticket_price
		                                                    FROM air_tickets) AS aT
	                                                    USING(air_ticket_id)
                                                    ) AS t
                                                    USING(order_id)
                                                    JOIN
	                                                    (SELECT airlines.id AS airlines_id, airlines.name AS airline_name FROM airlines) AS airl
                                                    USING(airlines_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS from_city_id, cities.name AS from_city_name FROM cities) AS frC
                                                    USING(from_city_id) 
                                                    JOIN 
	                                                    (SELECT cities.id AS to_city_id, cities.name AS to_city_name from cities) AS toC
                                                    USING(to_city_id) 
                                                    JOIN
	                                                    (SELECT hotel_orders.id AS hotel_order_id, hotel_orders.check_in, hotel_orders.check_out, hotel_orders.hotel_id, hotel_orders.price AS hotel_order_price
		                                                       FROM hotel_orders) AS hO
                                                    USING(hotel_order_id)
                                                    JOIN
	                                                    (SELECT hotels.id AS hotel_id, hotels.name AS hotel_name, hotels.city_id AS hotel_city_id, hotels.address, cities.name AS hotel_city_name 
	                                                     FROM hotels 
	                                                     JOIN cities ON hotels.city_id = cities.id) AS hotel
                                                    USING(hotel_id)", connection))
                {
                    cmd.Parameters.AddWithValue("cId", searchCart.Cart.Id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CartItem
                            {
                                Id = reader.GetInt32(7),
                                Cart = new Cart
                                {
                                    Id = reader.GetInt32(6),
                                    UserId = reader.GetInt32(8),
                                    Datetime = reader.GetDateTime(9)
                                },
                                Order = new Order
                                {
                                    Id = reader.GetInt32(5),
                                    AirTicket = new AirTicket
                                    {
                                        Id = reader.GetInt32(10),
                                        Depart = reader.GetDateTime(11),
                                        Arrive = reader.GetDateTime(12),
                                        FromCity = new City
                                        {
                                            Id = reader.GetInt32(3),
                                            Name = reader.GetString(15)
                                        },
                                        ToCity = new City
                                        {
                                            Id = reader.GetInt32(2),
                                            Name = reader.GetString(16)
                                        },
                                        Airline = new Airline
                                        {
                                            Id = reader.GetInt32(4),
                                            Name = reader.GetString(14)
                                        },
                                        Price = reader.GetDecimal(13)
                                    },
                                    HotelOrder = new HotelOrder
                                    {
                                        Id = reader.GetInt32(1),
                                        CheckIn = reader.GetDateTime(17),
                                        CheckOut = reader.GetDateTime(18),
                                        Hotel = new Hotel
                                        {
                                            Id = reader.GetInt32(0),
                                            Name = reader.GetString(20),
                                            City = new City
                                            {
                                                Id = reader.GetInt32(21),
                                                Name = reader.GetString(23)
                                            },
                                            Address = reader.GetString(22)
                                        },
                                        Price = reader.GetDecimal(19)
                                    }
                                }
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public int Add(CartItem cartItem)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO cart_items (cart_id, order_id) VALUES (@cartId, @orderId) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("cartId", cartItem.Cart.Id);
                        cmd.Parameters.AddWithValue("orderId", cartItem.Order.Id);

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

        public void Update(CartItem cartItem)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE cart_items SET (cart_id, order_id) = (@cartId, @orderId) WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", cartItem.Id);
                    cmd.Parameters.AddWithValue("cartId", cartItem.Cart.Id);
                    cmd.Parameters.AddWithValue("orderId", cartItem.Order.Id);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            CartItem deleteItem = Get(id);

            if (deleteItem == null || deleteItem.Order == null)
                return;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM cart_items WHERE Id = @id", connection))
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

            OrdersContext ordersContext = new OrdersContext();
            ordersContext.Delete(deleteItem.Order.Id);
        }
        
        public int Count(int id)
        {
            int result = 0;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM cart_items WHERE cart_id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            result = reader.GetInt32(0);
                    }
                }

                connection.Close();
            }

            return result;
        }

    }
}