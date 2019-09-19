using System;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    internal static partial class DbActions
    {
        /// <summary>
        /// Retrieves a single value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="query">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values.</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>The first column of the first row in the result set, or a null 
        /// reference if the result set is empty.</returns>
        public static object QGetScalar(
            string query,
            SqlParameter[] parameters,
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

                    if (parameters != null && parameters.Length > 0)
                    {
                        DbNullifyParameters(parameters);
                        command.Parameters.AddRange(parameters);
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

        /// <summary>
        /// Retrieves a single value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be 
        /// modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of 
        /// the stored procedure.</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>The first column of the first row in the result set, or a null 
        /// reference if the result set is empty.</returns>
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
