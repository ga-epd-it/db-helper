using System.Data.SqlClient;
using static EpdIt.DbActions;

namespace EpdIt
{
    public partial class DBHelper
    {
        public int SPReturnValue(
            string spName,
            SqlParameter[] parameters,
            out int rowsAffected)
        {
            rowsAffected = SPExecuteNonQuery(spName, parameters, out int returnValue, _connectionString);
            return returnValue;
        }

        public int SPReturnValue(
            string spName,
            SqlParameter parameter,
            out int rowsAffected)
        {
            SqlParameter[] parameters = parameter == null ? null : new SqlParameter[] { parameter };
            return SPReturnValue(spName, parameters, out rowsAffected);
        }

        public int SPReturnValue(
            string spName)
        {
            SqlParameter dummy = null;
            return SPReturnValue(spName, dummy, out _);
        }

        public bool SPRunCommand(
            string spName,
            SqlParameter[] parameters,
            out int rowsAffected,
            out int returnValue)
        {
            returnValue = SPReturnValue(spName, parameters, out rowsAffected);
            return returnValue == 0;
        }

        public bool SPRunCommand(
            string spName,
            SqlParameter[] parameters)
        {
            int returnValue = SPReturnValue(spName, parameters, out _);
            return returnValue == 0;
        }

        public bool SPRunCommand(
            string spName,
            SqlParameter parameter,
            out int rowsAffected,
            out int returnValue)
        {
            returnValue = SPReturnValue(spName, parameter, out rowsAffected);
            return returnValue == 0;
        }

        public bool SPRunCommand(string spName)
        {
            int returnValue = SPReturnValue(spName);
            return returnValue == 0;
        }
    }
}
