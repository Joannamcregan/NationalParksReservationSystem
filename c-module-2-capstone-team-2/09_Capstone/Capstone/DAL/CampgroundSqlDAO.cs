using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        public CampgroundSqlDAO(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

        public IList<Campground> GetAllCampgrounds(Park SelectedPark)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @ID ORDER BY campground_id", connection);
                    cmd.Parameters.AddWithValue("@ID", SelectedPark.Id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Campground> Campgrounds = new List<Campground>();

                    while (reader.Read())
                    {
                        Campgrounds.Add(RowToObject(reader));
                    }
                    return Campgrounds;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private Campground RowToObject(SqlDataReader reader)
        {
            Campground campground = new Campground();
            campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            campground.ParkId = Convert.ToInt32(reader["park_id"]);
            campground.CampName = Convert.ToString(reader["name"]);
            campground.Open_From_MM = Convert.ToInt32(reader["open_from_mm"]);
            campground.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
            campground.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

            return campground;
        }
    }
}