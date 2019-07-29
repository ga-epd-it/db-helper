using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EpdIt.DBHelperTest.Common
{
    internal static class TestDatabaseHelper
    {
        private const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        public static DBHelper GetDBHelper(this object callingClass, string appendToName = null, [CallerMemberName] string callingMember = null)
        {
            string dbName = $"{callingClass.GetType().Name}_{callingMember}_{appendToName}_Test";

            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
            string dbFileName = Path.Combine(outputFolder, $"{dbName}.mdf");
            string logFileName = Path.Combine(outputFolder, $"{dbName}_log.ldf");

            Directory.CreateDirectory(outputFolder);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DROP DATABASE IF EXISTS {dbName}";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { }

                    File.Delete(dbFileName);
                    File.Delete(logFileName);

                    cmd.CommandText = $"CREATE DATABASE {dbName} ON PRIMARY (NAME = N'{dbName}', FILENAME = N'{dbFileName}')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"exec sp_detach_db '{dbName}', 'true'";
                    cmd.ExecuteNonQuery();
                }
            }

            return new DBHelper(connectionString);
        }
    }
}
