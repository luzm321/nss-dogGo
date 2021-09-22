using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET/List: All Walks in db:
        public List<Walk> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT w.Id, w.Date, w.WalkerId, w.DogId, w.Duration, wk.Name [Walker Name], d.Name [Dog Name]
                                      FROM Walks w
                                      INNER JOIN Walker wk
                                      ON w.WalkerId = wk.Id
                                      INNER JOIN Dog d
                                      ON w.DogId = d.Id";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration"))
                            };

                            // If there is a WalkerId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("WalkerId")))
                            {
                                walk.Walker = new Walker
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Walker Name"))
                                };
                            }

                            // If there is a DogId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                            {
                                walk.Dog = new Dog
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Dog Name"))
                                };
                            }

                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }
        }

        // GET: Walk by Id
        public Walk GetWalkById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT w.Id, 
                                               w.Date, 
                                               w.WalkerId, 
                                               w.DogId, 
                                               w.Duration
                                               wk.Name [Walker Name],
                                               d.Name [Dog Name]
                                      FROM Walks w
                                      INNER JOIN Walker wk
                                      ON w.WalkerId = wk.Id
                                      INNER JOIN Dog d 
                                      ON w.DogId = d.Id
                                      WHERE w.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Walk walk = null;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (walk == null)
                            {
                                walk = new Walk
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                    DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                };
                            }
                            // If there is a DogId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                            {
                                walk.Dog = new Dog
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Dog Name")),
                                };
                            }
                            // If there is a WalkerId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("WalkerId")))
                            {
                                walk.Walker = new Walker
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Walker Name"))
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }

                        return walk;
                    }
                }
            }
        }

        // CREATE/ADD Walk:
        public void AddWalk(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Walks (Date, WalkerId, DogId, Duration)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @walkerId, @dogId, @duration);
                ";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }

        // UPDATE: Walk
        public void UpdateWalk(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Walks
                            SET 
                                Date = @date, 
                                WalkerId = @walkerId, 
                                DogId = @dogId, 
                                Duration = @duration
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@id", walk.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
