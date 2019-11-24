using System;
using System.Data.SqlClient;

namespace Capstone
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int MakeReservation(int siteId, string name, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@id, @name, @fromDate, @toDate)", connection);
                    cmd.Parameters.AddWithValue("@id", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.ExecuteNonQuery();

                    SqlCommand getId = new SqlCommand("SELECT @@IDENTITY", connection);

                    int newID = Convert.ToInt32(getId.ExecuteScalar());

                    return newID;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}