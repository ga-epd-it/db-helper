using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EpdIt
{
    internal static partial class DbActions
    {
        public static bool QExecuteNonQuery(
            List<string> queryList,
            List<SqlParameter[]> parametersList,
            out List<int> rowsAffectedList,
            string connectionString)
        {
            if (queryList.Count != parametersList.Count)
            {
                throw new ArgumentException("The number of queries does not match the number of SqlParameter sets");
            }

            if (queryList.Count == 0)
            {
                throw new ArgumentException("At least one query must be specified.", nameof(queryList));
            }

            rowsAffectedList = new List<int>();
            rowsAffectedList.Clear();

            bool success = true;
            int rowsAffected;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.Transaction = transaction;

                            for (int i = 0; i < queryList.Count; i++)
                            {
                                command.CommandText = queryList[i];

                                if (parametersList[i] != null)
                                {
                                    DbNullifyParameters(parametersList[i]);
                                    command.Parameters.AddRange(parametersList[i]);
                                }

                                rowsAffected = command.ExecuteNonQuery();

                                rowsAffectedList.Insert(i, rowsAffected);
                                command.Parameters.Clear();
                            }
                        }
                    }
                    catch (SqlException ee)
                    {
                        success = false;
                        rowsAffectedList.Clear();
                        throw ee;
                    }
                    finally
                    {
                        if (success)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            if (transaction != null)
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                }
                connection.Close();
            }

            return success;
        }

        public static int SPExecuteNonQuery(
            string spName,
            SqlParameter[] parameters,
            out int returnValue,
            string connectionString)
        {
            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentException("The name of the stored procedure must be specified.", nameof(spName));
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    // Set up
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        DbNullifyParameters(parameters);
                        command.Parameters.AddRange(parameters);
                    }

                    SqlParameter returnParameter = ReturnValueParameter();
                    command.Parameters.Add(returnParameter);

                    // Run
                    command.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    command.Connection.Close();

                    // Clean up
                    returnValue = (int)returnParameter.Value;
                    command.Parameters.Remove(returnParameter);

                    if (parameters != null && parameters.Length > 0)
                    {
                        SqlParameter[] newArray = new SqlParameter[command.Parameters.Count];
                        command.Parameters.CopyTo(newArray, 0);
                        Array.Copy(newArray, parameters, parameters.Length);
                    }

                    command.Parameters.Clear();

                    // Return
                    return rowsAffected;
                }
            }
        }
    }
}
