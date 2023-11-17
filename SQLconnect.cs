using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeAttendanceTracker
{
    public class SQLconnect
    {
        public void UploadTimeLog(int employeeID, string employeeName, int totalActiveTime)
        {
            // Define Azure SQL Server connection details
            string password = "Krakenj2010.";

            string connectionString = $"Server=tcp:krakeneat.database.windows.net,1433;Initial Catalog=Kraken;Persist Security Info=False;User ID=CloudSA5f676499;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO EmployeeAttendance (DateAttended, EmployeeID, EmployeeName, TimeWorked) VALUES (@DateAttended, @EmployeeID, @EmployeeName, @TimeWorked)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Get the current date
                        DateTime currentDate = DateTime.Now.Date;

                        command.Parameters.AddWithValue("@DateAttended", currentDate);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@EmployeeName", employeeName);
                        command.Parameters.AddWithValue("@TimeWorked", totalActiveTime);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Time log uploaded to Azure SQL Server successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload time log.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while uploading the time log: " + ex.Message);
            }
        }
    }
}
