using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class HotelsContext : Context
    {
        public IEnumerable<Hotel> GetAll()
        {
            List<Hotel> list = new List<Hotel>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM hotels LEFT JOIN cities ON hotels.city_id = cities.id", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(new Hotel
                            {
                                Id = Int32.Parse(reader.GetString(0)),
                                Name = reader.GetString(1),
                                City = new City
                                {
                                    Id = Int32.Parse(reader.GetString(2)),
                                    Name = reader.GetString(5)
                                },
                                Address = reader.GetString(3)
                            });
                    }
                }

                connection.Close();
            }

            return list;
        }

        public Hotel Get(int id)
        {
            Hotel hotel = new Hotel
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT t.id, t.name, t.city_id, t.address, cities.name FROM (SELECT * FROM hotels WHERE Id = @id) AS t JOIN cities ON t.city_id = cities.id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hotel.Name = reader.GetString(1);
                            hotel.City = new City
                            {
                                Id = Int32.Parse(reader.GetString(2)),
                                Name = reader.GetString(4)
                            };
                            hotel.Address = reader.GetString(3);
                        }
                    }
                }

                connection.Close();
            }

            return hotel;
        }

        public int Add(Hotel hotel)
        {
            int id = 0;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO hotels (name, city_id, address) VALUES (@n, @cId, @address) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("n", hotel.Name);
                        cmd.Parameters.AddWithValue("cId", hotel.City.Id);
                        cmd.Parameters.AddWithValue("address", hotel.Address);

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

        public void Update(Hotel hotel)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE hotels SET (name, city_id, address) = (@n, @cId, @address) WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", hotel.Id);
                    cmd.Parameters.AddWithValue("n", hotel.Name);
                    cmd.Parameters.AddWithValue("cId", hotel.City.Id);
                    cmd.Parameters.AddWithValue("address", hotel.Address);
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM hotels WHERE Id = @id", connection))
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