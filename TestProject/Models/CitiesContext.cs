using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class CitiesContext : Context
    {
        public IEnumerable<City> GetAll()
        {
            List<City> list = new List<City>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                
                using (var cmd = new NpgsqlCommand("SELECT * FROM cities", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new City
                            {
                                Id = Int32.Parse(reader.GetString(0)),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public City Get(int id)
        {
            City city = new City
            {
                Id = id 
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT name FROM cities WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            city.Name = reader.GetString(0);
                    }
                }

                connection.Close();
            }

            return city;
        }

        public int Add(string name)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO cities (name) VALUES (@n) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("n", name);

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

        public void Update(City city)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE cities SET name = @n WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", city.Id);
                    cmd.Parameters.AddWithValue("n", city.Name);
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM cities WHERE Id = @id", connection))
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