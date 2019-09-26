using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        public bool RunCommand(
            List<string> queryList,
            List<SqlParameter[]> parametersList,
            out List<int> rowsAffectedList) =>
            QExecuteNonQuery(queryList, parametersList, out rowsAffectedList, _connectionString);

        public bool RunCommand(
            List<string> queryList,
            List<SqlParameter[]> parametersList) =>
            RunCommand(queryList, parametersList, out _);

        public bool RunCommand(
            string query,
            SqlParameter[] parameters,
            out int rowsAffected)
        {
            var queryList = new List<string>() { query };
            var parameterArrayList = new List<SqlParameter[]>() { parameters };

            bool result = RunCommand(queryList, parameterArrayList, out List<int> countList);

            rowsAffected = 0;

            if (result && countList.Count > 0)
            {
                rowsAffected = countList[0];
            }

            return result;
        }

        public bool RunCommand(
            string query, 
            SqlParameter parameter, 
            out int rowsAffected)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return RunCommand(query, parameters, out rowsAffected);
        }

        public bool RunCommand(
            string query, 
            SqlParameter parameter = null)
        {
            return RunCommand(query, parameter, out _);
        }
    }
}
