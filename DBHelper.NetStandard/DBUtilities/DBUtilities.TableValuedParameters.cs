using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EpdIt
{
    public static partial class DBUtilities
    {

        /// <summary>
        /// Converts an IEnumerable of T to an IEnumerable of SqlDataRecord.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="values">The IEnumerable to convert.</param>
        /// <returns>An IEnumerable of SqlDataRecord.</returns>
        /// <remarks>See http://stackoverflow.com/a/337864/212978 </remarks>
        private static IEnumerable<SqlDataRecord> SqlDataRecords<T>(IEnumerable<T> values, string dbColumnName)
        {
            if (values == null || !values.Any())
                return null;

            SqlMetaData metadata;

            if (typeof(T) == typeof(string))
                // See https://stackoverflow.com/questions/337704/parameterize-an-sql-in-clause/337864#comment86087763_337864
                metadata = new SqlMetaData(dbColumnName, SqlDbType.NVarChar, -1);
            else
                metadata = SqlMetaData.InferFromValue(values.First(), dbColumnName);

            return values.Select(v =>
            {
                SqlDataRecord r = new SqlDataRecord(metadata);
                r.SetValues(v);
                return r;
            });
        }

        /// <summary>
        /// Returns a Structured (table-valued) SQL parameter using the IEnumerable values provided
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="parameterName">The SqlParameter name.</param>
        /// <param name="values">The IEnumerable to set as the value of the SqlParameter</param>
        /// <param name="dbTableTypeName">The name of the Table Type in the database</param>
        /// <returns>A table-valued SqlParameter of type T, containing the supplied values.</returns>
        /// <remarks>See http://stackoverflow.com/a/337864/212978 </remarks>
        public static SqlParameter TvpSqlParameter<T>(string parameterName, IEnumerable<T> values, string dbTableTypeName, string dbColumnName) =>
            new SqlParameter(parameterName, SqlDbType.Structured)
            {
                Value = SqlDataRecords(values, dbColumnName),
                TypeName = dbTableTypeName
            };
    }
}
