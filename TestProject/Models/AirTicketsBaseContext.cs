using System;
using System.Collections.Generic;
using Npgsql;

namespace TestProject.Models
{
    public class AirTicketsBaseContext : Context
    {
        public IEnumerable<AirTicket> GetAll()
        {
            List<AirTicket> list = new List<AirTicket>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT air_tickets_base.*, airlines.name, t1.name, t2.name 
                                                     FROM air_tickets_base 
                                                     JOIN airlines ON air_tickets_base.airlines_id = airlines.id 
                                                     JOIN cities t1 ON air_tickets_base.from_city_id = t1.id 
                                                     JOIN cities t2 ON air_tickets_base.to_city_id = t2.id", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                            list.Add(new AirTicket
                            {
                                Id = reader.GetInt32(0),
                                Depart = reader.GetDateTime(1),
                                Arrive = reader.GetDateTime(2),
                                FromCity = new City
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(8)
                                },
                                ToCity = new City
                                {
                                    Id = reader.GetInt32(4),
                                    Name = reader.GetString(9)
                                },
                                Airline = new Airline
                                {
                                    Id = reader.GetInt32(5),
                                    Name = reader.GetString(7)
                                },
                                Price = reader.GetDecimal(6)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public AirTicket Get(int id)
        {
            AirTicket airTicket = new AirTicket
            {
                Id = id
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(@"SELECT t.*, airlines.name, t1.name, t2.name
                                                     FROM 
                                                     (SELECT * FROM air_tickets_base WHERE air_tickets_base.id = @id) AS t 
                                                     JOIN airlines ON t.airlines_id = airlines.id
                                                     JOIN cities t1 ON t.from_city_id = t1.id
                                                     JOIN cities t2 ON t.to_city_id = t2.id", connection))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airTicket.Depart = reader.GetDateTime(1);
                            airTicket.Arrive = reader.GetDateTime(2);
                            airTicket.FromCity = new City
                            {
                                Id = reader.GetInt32(3),
                                Name = reader.GetString(8)
                            };
                            airTicket.ToCity = new City
                            {
                                Id = reader.GetInt32(4),
                                Name = reader.GetString(9)
                            };
                            airTicket.Airline = new Airline
                            {
                                Id = reader.GetInt32(5),
                                Name = reader.GetString(7)
                            };
                            airTicket.Price = reader.GetDecimal(6);
                        }
                    }
                }

                connection.Close();
            }

            return airTicket;
        }

        public IEnumerable<AirTicket> Get(AirTicket searchAirTicket)
        {
            List<AirTicket> list = new List<AirTicket>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();                

                using (var cmd = new NpgsqlCommand(@"SELECT t.*, airlines.name, t1.name, t2.name 
                                                     FROM 
                                                     (
	                                                     SELECT * FROM air_tickets_base 
	                                                     WHERE from_city_id = @fCid AND to_city_id = @tCid 
                                                               AND depart >= @dep ::date AND depart < @dep ::date + '1 day'::interval                                                               
                                                     ) AS t 
                                                     JOIN airlines ON t.airlines_id = airlines.id 
                                                     JOIN cities t1 ON t.from_city_id = t1.id 
                                                     JOIN cities t2 ON t.to_city_id = t2.id", connection))
                                                     
                                                     //AND arrive >= @arr ::date AND arrive < @arr ::date + '1 day'::interval
                {
                    cmd.Parameters.AddWithValue("dep", searchAirTicket.Depart);
                    //cmd.Parameters.AddWithValue("arr", searchAirTicket.Arrive);
                    cmd.Parameters.AddWithValue("fCid", searchAirTicket.FromCity.Id);
                    cmd.Parameters.AddWithValue("tCid", searchAirTicket.ToCity.Id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new AirTicket
                            {
                                Id = reader.GetInt32(0),
                                Depart = reader.GetDateTime(1),
                                Arrive = reader.GetDateTime(2),
                                FromCity = new City
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(8)
                                },
                                ToCity = new City
                                {
                                    Id = reader.GetInt32(4),
                                    Name = reader.GetString(9)
                                },
                                Airline = new Airline
                                {
                                    Id = reader.GetInt32(5),
                                    Name = reader.GetString(7)
                                },
                                Price = reader.GetDecimal(6)
                            });
                        }
                    }
                }

                connection.Close();
            }

            return list;
        }

        public int Add(AirTicket airTicket)
        {
            int id = 0;
            
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO air_tickets_base (depart, arrive, from_city_id, to_city_id, airlines_id, price) VALUES (@dep, @arr, @fCid, @tCid, @aId, @price) RETURNING Id", connection))
                    {
                        cmd.Parameters.AddWithValue("dep", airTicket.Depart);
                        cmd.Parameters.AddWithValue("arr", airTicket.Arrive);
                        cmd.Parameters.AddWithValue("fCid", airTicket.FromCity.Id);
                        cmd.Parameters.AddWithValue("tCid", airTicket.ToCity.Id);
                        cmd.Parameters.AddWithValue("aId", airTicket.Airline.Id);
                        cmd.Parameters.AddWithValue("price", airTicket.Price);

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

        public void Update(AirTicket airTicket)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("UPDATE air_tickets_base SET (depart, arrive, from_city_id, to_city_id, airlines_id, price) = (@dep, @arr, @fCid, @tCid, @aId, @price) WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("id", airTicket.Id);
                    cmd.Parameters.AddWithValue("dep", airTicket.Depart);
                    cmd.Parameters.AddWithValue("arr", airTicket.Arrive);
                    cmd.Parameters.AddWithValue("fCid", airTicket.FromCity.Id);
                    cmd.Parameters.AddWithValue("tCid", airTicket.ToCity.Id);
                    cmd.Parameters.AddWithValue("aId", airTicket.Airline.Id);
                    cmd.Parameters.AddWithValue("price", airTicket.Price);
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM air_tickets_base WHERE Id = @id", connection))
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