using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id, o.Email, o.Name, o.Address, o.NeighborhoodId, o.Phone, n.Name [Neighborhood Name]
                                      FROM Owner o
                                      INNER JOIN Neighborhood n
                                      ON o.NeighborhoodId = n.Id";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Owner> owners = new List<Owner>();
                        while (reader.Read())
                        {
                            Owner owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone"))
                            };

                            // If there is a NeighborhoodId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("NeighborhoodId")))
                            {
                                owner.Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Neighborhood Name"))
                                };
                            }

                            owners.Add(owner);
                        }

                        return owners;
                    }
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id, o.Email, o.Name, o.Address, o.NeighborhoodId, o.Phone, n.Name [Neighborhood Name]
                                      FROM Owner o
                                      INNER JOIN Neighborhood n
                                      ON o.NeighborhoodId = n.Id
                                      WHERE o.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Owner owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone"))
                            };

                            // If there is a NeighborhoodId in the database:
                            if (!reader.IsDBNull(reader.GetOrdinal("NeighborhoodId")))
                            {
                                owner.Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Neighborhood Name"))
                                };
                            }

                            return owner;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
