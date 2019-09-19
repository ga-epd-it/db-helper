using System;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        /// <summary>
        /// Determines whether a value as indicated by the SQL query exists in the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A boolean value signifying whether the indicated value exists and is not DBNull.</returns>
        public bool ValueExists(string query, SqlParameter[] parameters)
        {
            object result = QGetScalar(query, parameters, connectionString);
            return (result != null && !Convert.IsDBNull(result));
        }

        /// <summary>
        /// Determines whether a value as indicated by the SQL query exists in the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter array to send.</param>
        /// <returns>A boolean value signifying whether the indicated value exists and is not DBNull.</returns>
        public bool ValueExists(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return ValueExists(query, parameters);
        }
    }
}
