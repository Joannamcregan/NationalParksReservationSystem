using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;

        public SiteSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Site> ListAvailableSites(int selectedCampgroundId, DateTime arrivalDate, DateTime departureDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("select TOP 5 * from Site s " +
                        "JOIN Campground as C on s.campground_id = C.campground_id " +
                        "where s.campground_id = @campgroundId and site_id not in " +
                        "(select site_id from Reservation r where r.from_date <= @toDate AND @fromDate <= r.to_date) "
                        , connection);
                    cmd.Parameters.AddWithValue("@campgroundId", selectedCampgroundId);
                    cmd.Parameters.AddWithValue("@fromDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@toDate", departureDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Site> availableSites = new List<Site>();

                    while (reader.Read())
                    {
                        availableSites.Add(RowToObject(reader));
                    }
                    return availableSites;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IList<Site> ListAvailableSitesInPark(Park selectedPark, DateTime desiredArrival, DateTime desiredDeparture)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("select * from Site s " +
                        "JOIN Campground as C on s.campground_id = C.campground_id " +
                        "JOIN park p ON p.park_id = c.park_id " +
                        "where p.park_id = @ParkId and site_id not in " +
                        "(select site_id from Reservation r where r.from_date <= @toDate AND @fromDate <= r.to_date) "
                        , connection);
                    cmd.Parameters.AddWithValue("@ParkId", selectedPark.Id);
                    cmd.Parameters.AddWithValue("@fromDate", desiredArrival);
                    cmd.Parameters.AddWithValue("@toDate", desiredDeparture);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Site> availableSites = new List<Site>();

                    while (reader.Read())
                    {
                        availableSites.Add(RowToObject(reader));
                    }
                    return availableSites;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private Site RowToObject(SqlDataReader reader)
        {
            Site site = new Site();
            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToInt32(reader["accessible"]); //1 is true 0 is false
            site.MaxLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToInt32(reader["Utilities"]);
            site.CostPerDay = Convert.ToDecimal(reader["daily_fee"]);
            site.CampgroundName = Convert.ToString(reader["name"]);
            return site;
        }
    }
}