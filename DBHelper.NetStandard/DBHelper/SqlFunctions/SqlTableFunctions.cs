using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        // DataTable

        public DataTable GetDataTable(string query, SqlParameter[] parameters)
        {
            return QFillDataTable(query, parameters, connectionString);
        }

        public DataTable GetDataTable(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetDataTable(query, parameters);
        }

        // DataRow

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

        public DataRow GetDataRow(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetDataRow(query, parameters);
        }

        // LookupDictionary

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

        public Dictionary<int, string> GetLookupDictionary(string query, SqlParameter parameter = null)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return GetLookupDictionary(query, parameters);
        }
    }
}
