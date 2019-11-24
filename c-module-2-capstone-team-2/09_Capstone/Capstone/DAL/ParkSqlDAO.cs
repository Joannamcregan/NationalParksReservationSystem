using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone
{
    public class ParkSqlDAO : IParkDAO
    {
        private string connectionString;

        public ParkSqlDAO(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

        public IList<Park> GetAllParks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park ORDER BY name", connection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Park> Parks = new List<Park>();

                    while (reader.Read())
                    {
                        Parks.Add(RowToObject(reader));
                    }
                    return Parks;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public Park GetInfo(int parkID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE park_id = @ID", connection);
                    cmd.Parameters.AddWithValue("@ID", parkID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Park park = new Park();

                    while (reader.Read())
                    {
                        park.Id = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                    }
                    return park;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private Park RowToObject(SqlDataReader reader)
        {
            Park park = new Park();
            park.Id = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }
    }
}