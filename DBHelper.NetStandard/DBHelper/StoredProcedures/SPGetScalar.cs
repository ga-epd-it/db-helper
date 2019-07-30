using System;
using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {

        /// <summary>
        /// Retrieves a single value of the specified type from the database by calling a stored procedure.
        /// </summary>
        /// <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A value of the specified type.</returns>
        public T SPGetSingleValue<T>(string spName, SqlParameter[] parameters, out int returnValue)
        {
            var result = SPExecuteScalar(spName, parameters, out returnValue, connectionString);
            return DBUtilities.GetNullable<T>(result);
        }

        /// <summary>
        /// Retrieves a single value of the specified type from the database by calling a stored procedure.
        /// </summary>
        /// <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A value of the specified type.</returns>
        public T SPGetSingleValue<T>(string spName, SqlParameter parameter, out int returnValue)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPGetSingleValue<T>(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a single value of the specified type from the database by calling a stored procedure.
        /// </summary>
        /// <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">A SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A value of the specified type.</returns>
        public T SPGetSingleValue<T>(string spName, SqlParameter parameter = null)
        {
            return SPGetSingleValue<T>(spName, parameter, out _);
        }

        /// <summary>
        /// Retrieves a scalar boolean value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A boolean value.</returns>
        public bool SPGetBoolean(string spName, SqlParameter[] parameters, out int returnValue)
        {
            return Convert.ToBoolean(SPGetSingleValue<bool>(spName, parameters, out returnValue));
        }

        /// <summary>
        /// Retrieves a scalar boolean value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A boolean value.</returns>
        public bool SPGetBoolean(string spName, SqlParameter parameter, out int returnValue)
        {
            return Convert.ToBoolean(SPGetSingleValue<bool>(spName, parameter, out returnValue));
        }

        /// <summary>
        /// Retrieves a scalar boolean value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A boolean value.</returns>
        public bool SPGetBoolean(string spName, SqlParameter parameter = null)
        {
            return Convert.ToBoolean(SPGetSingleValue<bool>(spName, parameter, out _));
        }

        /// <summary>
        /// Retrieves a scalar integer value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>An integer value.</returns>
        public int SPGetInteger(string spName, SqlParameter[] parameters, out int returnValue)
        {
            return SPGetSingleValue<int>(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a scalar integer value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>An integer value.</returns>
        public int SPGetInteger(string spName, SqlParameter parameter, out int returnValue)
        {
            return SPGetSingleValue<int>(spName, parameter, out returnValue);
        }

        /// <summary>
        /// Retrieves a scalar integer value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>An integer value.</returns>
        public int SPGetInteger(string spName, SqlParameter parameter = null)
        {
            return SPGetSingleValue<int>(spName, parameter, out _);
        }

        /// <summary>
        /// Retrieves a scalar string value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of SqlParameter values. The array may be modified by the stored produre if it includes output parameters.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A string value.</returns>
        public string SPGetString(string spName, SqlParameter[] parameters, out int returnValue)
        {
            return SPGetSingleValue<string>(spName, parameters, out returnValue);
        }

        /// <summary>
        /// Retrieves a scalar string value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <param name="returnValue">Optional output parameter that stores the RETURN value of the stored procedure.</param>
        /// <returns>A string value.</returns>
        public string SPGetString(string spName, SqlParameter parameter, out int returnValue)
        {
            return SPGetSingleValue<string>(spName, parameter, out returnValue);
        }

        /// <summary>
        /// Retrieves a scalar string value from the database by calling a stored procedure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="parameter">An optional SqlParameter value. The value may be modified by the stored produre if it is an output parameter.</param>
        /// <returns>A string value.</returns>
        public string SPGetString(string spName, SqlParameter parameter = null)
        {
            return SPGetSingleValue<string>(spName, parameter, out _);
        }
    }
}
