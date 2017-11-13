using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class HotelOrdersContext : Context
    {
        public IEnumerable<HotelOrder> GetAll()
        {
            List<HotelOrder> list = new List<HotelOrder>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM hotel_orders 
                                                     JOIN (SELECT hotels.*, cities.name FROM hotels JOIN cities ON hotels.city_id = cities.id) AS h 
                                                     ON hotel_orders.hotel_id = h.id", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HotelOrder
                            { 
                                Id = reader.GetInt32(0),
                                CheckIn = reader.GetDateTime(1),
                                CheckOut = reader.GetDateTime(2),
                                Hotel = new Hotel
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(6),
                                    City = new City
                                    {
                                        Id = reader.GetInt32(7),
                                        Name = reader.GetString(9)
                                    },
                                    Address = reader.GetString(8)
                                },
                                Price = reader.GetDecimal(4)
                            }
                            );
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public HotelOrder Get(int id)
        {
            HotelOrder hotelOrder = new HotelOrder
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM 
                                                     (SELECT * FROM hotel_orders WHERE hotel_orders.id = @id) AS hO
                                                     JOIN (SELECT hotels.*, cities.name FROM hotels JOIN cities ON hotels.city_id = cities.id) AS h 
                                                     ON hO.hotel_id = h.id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hotelOrder.CheckIn = reader.GetDateTime(1);
                            hotelOrder.CheckOut = reader.GetDateTime(2);
                            hotelOrder.Hotel = new Hotel
                            {
                                Id = reader.GetInt32(3),
                                Name = reader.GetString(6),
                                City = new City
                                {
                                    Id = reader.GetInt32(7),
                                    Name = reader.GetString(9)
                                },
                                Address = reader.GetString(8)
                            };
                            hotelOrder.Price = reader.GetDecimal(4);
                        }
                    }
                }

                connection.Close();
            }

            return hotelOrder;
        }

        public IEnumerable<HotelOrder> Get(HotelOrder searchHotelOrder)
        {
            List<HotelOrder> list = new List<HotelOrder>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT * FROM 
                                                     (SELECT * FROM hotel_orders
                                                        WHERE check_in >= @cIn ::date AND check_in < @cIn ::date + '1 day'::interval
                                                              AND check_out >= @cOut ::date AND check_out < @cOut ::date + '1 day'::interval
                                                              AND hotel_id = hId) AS hO
                                                     JOIN (SELECT hotels.*, cities.name FROM hotels JOIN cities ON hotels.city_id = cities.id) AS h 
                                                     ON hO.hotel_id = h.id", connection))
                {
                    cmd.Parameters.AddWithValue("cIn", searchHotelOrder.CheckIn);
                    cmd.Parameters.AddWithValue("cOut", searchHotelOrder.CheckOut);
                    cmd.Parameters.AddWithValue("hId", searchHotelOrder.Hotel.Id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HotelOrder
                            {
                                CheckIn = reader.GetDateTime(1),
                                CheckOut = reader.GetDateTime(2),
                                Hotel = new Hotel
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(6),
                                    City = new City
                                    {
                                        Id = reader.GetInt32(7),
                                        Name = reader.GetString(9)
                                    },
                                    Address = reader.GetString(8)
                                },
                                Price = reader.GetDecimal(4)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public int Add(HotelOrder hotelOrder)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO hotel_orders (check_in, check_out, hotel_id, price) VALUES (@cIn, @cOut, @hId, @price) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("cIn", hotelOrder.CheckIn);
                        cmd.Parameters.AddWithValue("cOut", hotelOrder.CheckOut);
                        cmd.Parameters.AddWithValue("hId", hotelOrder.Hotel.Id);
                        cmd.Parameters.AddWithValue("price", hotelOrder.Price); //need calculate for hotel\dates

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
        
        public void Update(HotelOrder hotelOrder)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE hotel_orders SET (check_in, check_out, hotel_id, price) = (@cIn, @cOut, @hId, @price) WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", hotelOrder.Id);
                    cmd.Parameters.AddWithValue("cIn", hotelOrder.CheckIn);
                    cmd.Parameters.AddWithValue("cOut", hotelOrder.CheckOut);
                    cmd.Parameters.AddWithValue("hId", hotelOrder.Hotel.Id);
                    cmd.Parameters.AddWithValue("price", hotelOrder.Price);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM hotel_orders WHERE Id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                }

                connection.Close();
            }
        }
    }
}