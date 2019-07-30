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

                    cmd.CommandText = "create or alter procedure SProc @case varchar(1) = null as begin " +
                        "    if (@case is null) begin select 'null parameter'; return 0; end; " +
                        "    if (@case = 'A') begin select 0; return 10; end; " +
                        "    if (@case = 'B') begin select 1; return 11; end; " +
                        "    if (@case = 'C') begin select convert(date, '2014-05-01'); return 22; end; " +
                        "    if (@case = 'D') begin select convert(bit, 0); return 30; end; " +
                        "    if (@case = 'E') begin select convert(bit, 1); return 31; end; " +
                        "    select 'other'; return 99; " +
                        "end";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"exec sp_detach_db '{dbName}', 'true'";
                    cmd.ExecuteNonQuery();
                }
            }

            return new DBHelper(connectionString);
        }
    }
}
