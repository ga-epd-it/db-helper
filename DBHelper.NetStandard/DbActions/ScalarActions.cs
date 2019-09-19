using System;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    internal static partial class DbActions
    {
        public static object QGetScalar(
            string query, 
            SqlParameter[] parameterArray, 
            string connectionString)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("The query must be specified.", "query");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    // Set up
                    command.CommandType = CommandType.Text;

                    if (parameterArray != null && parameterArray.Length > 0)
                    {
                        DbNullifyParameters(parameterArray);
                        command.Parameters.AddRange(parameterArray);
                    }

                    // Run
                    command.Connection.Open();
                    object result = command.ExecuteScalar();
                    command.Connection.Close();

                    // Clean up
                    command.Parameters.Clear();

                    // Return
                    return result;
                }
            }
        }

        public static object SPExecuteScalar(
            string spName, 
            SqlParameter[] parameters, 
            out int returnValue, 
            string connectionString)
        {
            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentException("The name of the stored procedure must be specified.", "spName");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    // Set up
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        DbNullifyParameters(parameters);
                        command.Parameters.AddRange(parameters);
                    }

                    SqlParameter returnParameter = ReturnValueParameter();
                    command.Parameters.Add(returnParameter);

                    // Run
                    command.Connection.Open();
                    object result = command.ExecuteScalar();
                    command.Connection.Close();

                    // Clean up
                    returnValue = (int)returnParameter.Value;
                    command.Parameters.Remove(returnParameter);

                    if (parameters != null && parameters.Length > 0)
                    {
                        SqlParameter[] newArray = new SqlParameter[command.Parameters.Count];
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
