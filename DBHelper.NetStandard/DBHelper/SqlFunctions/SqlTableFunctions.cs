using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        // DataTable

        /// <summary>
        /// Retrieves a DataTable of values from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A DataTable of values.</returns>
        public DataTable GetDataTable(string query, SqlParameter[] parameters)
        {
            return QFillDataTable(query, parameters, connectionString);
        }

        /// <summary>
        /// Retrieves a DataTable of values from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A DataTable of values.</returns>
        public DataTable GetDataTable(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetDataTable(query, parameters);
        }

        // DataRow

        /// <summary>
        /// Retrieves a single row of values from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A DataRow of values.</returns>
        public DataRow GetDataRow(string query, SqlParameter[] parameters)
        {
            DataTable dataTable = GetDataTable(query, parameters);

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return null;
            }

            if (dataTable.Rows.Count > 1)
            {
                throw new TooManyRecordsException($"Query: '{query}'");
            }

            return dataTable.Rows[0];
        }

        /// <summary>
        /// Retrieves a single row of values from the database.
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A DataRow of values.</returns>
        public DataRow GetDataRow(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetDataRow(query, parameters);
        }

        // LookupDictionary

        /// <summary>
        /// Retrieves a dictionary of (integer -> string) values from the database
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameters">An array of SqlParameter values to send.</param>
        /// <returns>A lookup dictionary.</returns>
        public Dictionary<int, string> GetLookupDictionary(string query, SqlParameter[] parameters)
        {
            Dictionary<int, string> d = new Dictionary<int, string>();
            DataTable dataTable = GetDataTable(query, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                d.Add((int)row[0], (string)row[1]);
            }

            return d;
        }

        /// <summary>
        /// Retrieves a dictionary of (integer -> string) values from the database
        /// </summary>
        /// <param name="query">The SQL query to send.</param>
        /// <param name="parameter">An optional SqlParameter to send.</param>
        /// <returns>A lookup dictionary.</returns>
        public Dictionary<int, string> GetLookupDictionary(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetLookupDictionary(query, parameters);
        }
    }
}
