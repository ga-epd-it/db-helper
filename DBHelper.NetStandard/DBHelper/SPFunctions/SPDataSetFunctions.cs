using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        // DataSet

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataSet.</returns>
        public DataSet SPGetDataSet(string spName, SqlParameter[] parameters, out int returnValue)
        {
            return SPFillDataSet(spName, parameters, out returnValue, connectionString);
        }

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataSet.</returns>
        public DataSet SPGetDataSet(string spName, SqlParameter parameter, out int returnValue)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPGetDataSet(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A DataSet.</returns>
        public DataSet SPGetDataSet(string spName, SqlParameter parameter = null)
        {
            return SPGetDataSet(spName, parameter, out _);
        }

        // DataTable

        /// <summary>
        /// Retrieves a DataTable of values from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataTable.</returns>
        public DataTable SPGetDataTable(string spName, SqlParameter[] parameters, out int returnValue)
        {
            DataSet dataSet = SPGetDataSet(spName, parameters, out returnValue);

            if (dataSet == null)
            {
                return null;
            }

            if (dataSet.Tables.Count > 1)
            {
                throw new TooManyRecordsException($"Stored Procedure: '{spName}'");
            }

            return dataSet.Tables[0];
        }

        /// <summary>
        /// Retrieves a DataTable of values from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataTable.</returns>
        public DataTable SPGetDataTable(string spName, SqlParameter parameter, out int returnValue)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPGetDataTable(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a DataTable of values from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A DataTable.</returns>
        public DataTable SPGetDataTable(string spName, SqlParameter parameter = null)
        {
            return SPGetDataTable(spName, parameter, out _);
        }

        // DataRow

        /// <summary>
        /// Retrieves a single row of values from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataRow.</returns>
        public DataRow SPGetDataRow(string spName, SqlParameter[] parameters, out int returnValue)
        {
            DataTable dataTable = SPGetDataTable(spName, parameters, out returnValue);

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return null;
            }

            if (dataTable.Rows.Count > 1)
            {
                throw new TooManyRecordsException($"Stored Procedure: '{spName}'");
            }

            return dataTable.Rows[0];
        }

        /// <summary>
        /// Retrieves a single row of values from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataRow.</returns>
        public DataRow SPGetDataRow(string spName, SqlParameter parameter, out int returnValue)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPGetDataRow(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a single row of values from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataRow.</returns>
        public DataRow SPGetDataRow(string spName, SqlParameter parameter = null)
        {
            return SPGetDataRow(spName, parameter, out _);
        }

        // LookupDictionary

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataSet.</returns>
        public Dictionary<int, string> SPGetLookupDictionary(string spName, SqlParameter[] parameters, out int returnValue)
        {
            Dictionary<int, string> d = new Dictionary<int, string>();
            DataTable dataTable = SPGetDataTable(spName, parameters, out returnValue);

            foreach (DataRow row in dataTable.Rows)
            {
                d.Add((int)row[0], (string)row[1]);
            }

            return d;
        }

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A DataSet.</returns>
        public Dictionary<int, string> SPGetLookupDictionary(string spName, SqlParameter parameter, out int returnValue)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPGetLookupDictionary(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a DataSet containing one or more DataTables selected from the database.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A DataSet.</returns>
        public Dictionary<int, string> SPGetLookupDictionary(string spName, SqlParameter parameter = null)
        {
            return SPGetLookupDictionary(spName, parameter, out _);
        }
    }
}
