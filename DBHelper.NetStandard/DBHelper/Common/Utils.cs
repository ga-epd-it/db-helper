using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    public partial class DBHelper
    {
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

        private SqlParameter ReturnValueParameter() =>
            new SqlParameter("@DbHelperReturnValue", SqlDbType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };
    }
}
