using System;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        /// <summary>
        /// Retrieves a single value of the specified type from the database.
        /// </summary>
        /// <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A value of the specified type.</returns>
        public T GetSingleValue<T>(string query, SqlParameter[] parameters) =>
            DBUtilities.GetNullable<T>(QGetScalar(query, parameters, _connectionString));

        /// <summary>
        /// Retrieves a single value of the specified type from the database.
        /// </summary>
        /// <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A value of the specified type.</returns>
        public T GetSingleValue<T>(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetSingleValue<T>(query, parameters);
        }

        /// <summary>
        /// Retrieves a boolean scalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A boolean value.</returns>
        public bool GetBoolean(string query, SqlParameter parameter = null) =>
            Convert.ToBoolean(GetSingleValue<bool>(query, parameter));

        /// <summary>
        /// Retrieves a boolean sclalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameterArray">An array of SqlParameter values to send.</param>
        /// <returns>A boolean value.</returns>
        public bool GetBoolean(string query, SqlParameter[] parameters) =>
            Convert.ToBoolean(GetSingleValue<bool>(query, parameters));

        /// <summary>
        /// Retrieves an integer scalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>An integer value.</returns>
        public int GetInteger(string query, SqlParameter parameter = null) =>
            GetSingleValue<int>(query, parameter);

        /// <summary>
        /// Retrieves an integer sclalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameterArray">An array of SqlParameter values to send.</param>
        /// <returns>An integer value.</returns>
        public int GetInteger(string query, SqlParameter[] parameters) =>
            GetSingleValue<int>(query, parameters);

        /// <summary>
        /// Retrieves a string scalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A string value.</returns>
        public string GetString(string query, SqlParameter parameter = null) =>
            GetSingleValue<string>(query, parameter);

        /// <summary>
        /// Retrieves a string scalar value from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A string value.</returns>
        public string GetString(string query, SqlParameter[] parameters) =>
            GetSingleValue<string>(query, parameters);
    }
}
