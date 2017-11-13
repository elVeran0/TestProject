using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class AirlinesContext : Context
    {
        public IEnumerable<Airline> GetAll()
        {
            List<Airline> list = new List<Airline>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM airlines", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Airline
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

        public Airline Get(int id)
        {
            Airline airline = new Airline
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT name FROM airlines WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            airline.Name = reader.GetString(0);
                    }
                }

                connection.Close();
            }

            return airline;
        }

        public int Add(String name)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO airlines (name) VALUES (@name) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("name", name);

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

        public void Update(Airline airline)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE airlines SET name = @name WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", airline.Id);
                    cmd.Parameters.AddWithValue("name", airline.Name);
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM airlines WHERE Id = @id", connection))
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