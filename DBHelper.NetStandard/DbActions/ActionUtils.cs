using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    internal static partial class DbActions
    {
        /// <summary>
        /// Loops through all SqlParameters in an array and if the Value of any evaluates to Nothing, sets the Value 
        /// to DBNull.Value. This will force the parameter to be sent with the call to the database. 
        /// 
        /// (By default, parameters that evaluate to Nothing are removed from the database call, causing an 
        /// error if the parameter is expected, even if null is an allowed value.)
        /// </summary>
        /// <param name="parameters">An array of SqlParameter.</param>
        /// <returns>An array of SqlParameter.</returns>
        private static SqlParameter[] DbNullifyParameters(SqlParameter[] parameters)
        {
            List<SqlParameter> result = new List<SqlParameter>();

            foreach (SqlParameter parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }

                result.Add(parameter);
            }

            return result.ToArray();
        }

        private static SqlParameter ReturnValueParameter() =>
            new SqlParameter("@DbHelperReturnValue", SqlDbType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };
    }
}
