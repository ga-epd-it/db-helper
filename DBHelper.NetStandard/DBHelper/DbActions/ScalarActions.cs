using System;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    public partial class DBHelper
    {
        private object QGetScalar(string query, SqlParameter[] parameterArray)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("The query must be specified.", "query");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    // Setup
                    command.CommandType = CommandType.Text;

                    if (parameterArray != null && parameterArray.Length > 0)
                    {
                        DbNullifyParameters(parameterArray);
                        command.Parameters.AddRange(parameterArray);
                    }

                    // Run
                    command.Connection.Open();
                    var result = command.ExecuteScalar();
                    command.Connection.Close();

                    // Cleanup
                    command.Parameters.Clear();

                    // Return
                    return result;
                }
            }
        }
    }
}
