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

        private object SPExecuteScalar(string spName, SqlParameter[] parameters, out int returnValue)
        {
            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentException("The name of the stored procedure must be specified.", "spName");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    // Setup
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        DbNullifyParameters(parameters);
                        command.Parameters.AddRange(parameters);
                    }

                    var returnParameter = ReturnValueParameter();
                    command.Parameters.Add(returnParameter);

                    // Run
                    command.Connection.Open();
                    var result = command.ExecuteScalar();
                    command.Connection.Close();

                    // Cleanup
                    returnValue = Convert.ToInt32(returnParameter.Value);
                    command.Parameters.Remove(returnParameter);

                    if (parameters != null && parameters.Length > 0)
                    {
                        var newArray = new SqlParameter[command.Parameters.Count];
                        command.Parameters.CopyTo(newArray, 0);
                        Array.Copy(newArray, parameters, parameters.Length);
                    }

                    command.Parameters.Clear();

                    // Return
                    return result;
                }
            }
        }
    }
}
