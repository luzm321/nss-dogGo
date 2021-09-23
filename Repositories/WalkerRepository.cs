using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
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

        // GET: List of all walkers:
        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, n.Id [nId], n.Name [Neighborhood Name]
                        FROM Walker w
                        INNER JOIN Neighborhood n
                        ON w.NeighborhoodId = n.Id
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                        // If there is a NeighborhoodId in the database:
                        if (!reader.IsDBNull(reader.GetOrdinal("NeighborhoodId")))
                        {
                            walker.Neighborhood = new Neighborhood
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("Neighborhood Name"))
                            };
                        }

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        // GET: Walker object by id:
        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.Name [Walker Name], w.ImageUrl, w.NeighborhoodId, n.Id [nId], n.Name [Neighborhood Name], wk.Date, wk.Duration, o.Name [Owner Name]
                        FROM Walker w
                        LEFT JOIN Neighborhood n
                        ON w.NeighborhoodId = n.Id
                        LEFT JOIN Walks wk 
                        ON w.Id = wk.WalkerId
                        LEFT JOIN Dog d 
                        ON wk.DogId = d.Id
                        LEFT JOIN Owner o 
                        ON d.OwnerId = o.Id
                        WHERE w.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Walker walker = null;

                    while (reader.Read())
                    {
                        walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Walks = new List<Walk>()
                        };

                        // If there is a NeighborhoodId in the database:
                        if (!reader.IsDBNull(reader.GetOrdinal("NeighborhoodId")))
                        {
                            walker.Neighborhood = new Neighborhood
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("nId")),
                                Name = reader.GetString(reader.GetOrdinal("Neighborhood Name"))
                            };
                        }

                        // If there is a Date in the database:
                        if (!reader.IsDBNull(reader.GetOrdinal("Date")))
                        {
                            Walk walk = new Walk
                            {
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Client = new Owner 
                                { 
                                    Name = reader.GetString(reader.GetOrdinal("Owner Name")) 
                                }
                            };
                            walker.Walks.Add(walk);
                        }
                        else
                        {
                            reader.Close();
                            return null;
                        }

                        reader.Close();
                        return walker;
                    }
                }
            }
        }

        //GET: List of walkers in a neighborhood:
        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }
    }
}

