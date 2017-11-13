using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class CartsContext : Context
    {
        public IEnumerable<Cart> GetAll()
        {
            List<Cart> list = new List<Cart>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM carts", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Cart
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Datetime = reader.GetDateTime(2)                               
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public Cart Get(int id)
        {
            Cart cart = new Cart
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM carts WHERE carts.id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            cart.Id = reader.GetInt32(0);
                            cart.UserId = reader.GetInt32(1);
                            cart.Datetime = reader.GetDateTime(2);
                        }
                    }
                }

                connection.Close();
            }

            return cart;
        }

        public int Add(Cart cart)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO carts (user_id, datetime) VALUES (@uI, @dt) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("uI", cart.UserId);
                        cmd.Parameters.AddWithValue("dt", cart.Datetime);

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

        public void Update(Cart cart)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE carts SET (user_id, datetime) = (@uI, @dt) WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", cart.Id);
                    cmd.Parameters.AddWithValue("uI", cart.UserId);
                    cmd.Parameters.AddWithValue("dt", cart.Datetime);
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM carts WHERE Id = @id", connection))
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