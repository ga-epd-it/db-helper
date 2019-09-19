using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EpdIt.DBHelperTest.Common
{
    internal static class TestDatabaseHelper
    {
        private const string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        public const string spScalar = "spScalar";
        public const string spTables = "spTables";

        public const string tableQuery = @"
            WITH
                numbers AS (
                    SELECT 1 as [number], 
                           convert(nvarchar(max), concat('text-', 1)) as [text], 
                           cast(0x0 as uniqueidentifier) as [guid],
                           sysdatetimeoffset() as [date]
                    UNION ALL
                    SELECT number + 1,
                           convert(nvarchar(max), concat('text-', number + 1)),
                           newid(),
                           dateadd(month, -number, sysdatetimeoffset())
                    FROM numbers
                    WHERE number < 10)
            SELECT *
            FROM numbers
            OPTION (MAXRECURSION 0)";

        public const string tableQueryParam = @"
            WITH
                numbers AS (
                    SELECT 1 as [number], 
                           convert(nvarchar(max), concat('text-', 1)) as [text], 
                           cast(0x0 as uniqueidentifier) as [guid],
                           sysdatetimeoffset() as [date]
                    UNION ALL
                    SELECT number + 1,
                           convert(nvarchar(max), concat('text-', number + 1)),
                           newid(),
                           dateadd(month, -number, sysdatetimeoffset())
                    FROM numbers
                    WHERE number < @count)
            SELECT *
            FROM numbers
            OPTION (MAXRECURSION 0)";

        public static DBHelper GetDBHelper(this object callingClass, string appendToName = null, [CallerMemberName] string callingMember = null)
        {
            string dbName = $"{callingClass.GetType().Name}_{callingMember}_{appendToName}_Test";

            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
            string dbFileName = Path.Combine(outputFolder, $"{dbName}.mdf");
            string logFileName = Path.Combine(outputFolder, $"{dbName}_log.ldf");

            Directory.CreateDirectory(outputFolder);

            using (var connection = new SqlConnection(_connectionString))
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

                    cmd.CommandText = $@"create or alter procedure {spScalar} @case varchar(1) = null as begin
                            if (@case is null) begin select 'null parameter'; return 0; end;
                            if (@case = 'A') begin select 0; return 10; end;
                            if (@case = 'B') begin select 1; return 11; end;
                            if (@case = 'C') begin select convert(date, '2014-05-01'); return 22; end;
                            if (@case = 'D') begin select convert(bit, 0); return 30; end;
                            if (@case = 'E') begin select convert(bit, 1); return 31; end;
                            select 'other'; return 99;
                        end";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $@"create or alter procedure {spTables} @count int = 10, @dataSet bit = 0 as begin
                        WITH numbers AS (
                            SELECT 1 as [number], 
                                convert(nvarchar(max), concat('text-', 1)) as [text],
                                cast(0x0 as uniqueidentifier) as [guid],
                                sysdatetimeoffset() as [date]
                            UNION ALL
                            SELECT number + 1,
                                convert(nvarchar(max), concat('text-', number + 1)),
                                newid(),
                                dateadd(month, -number, sysdatetimeoffset())
                            FROM numbers WHERE number < @count)
                        SELECT * FROM numbers 
                        OPTION (MAXRECURSION 0);

                        if @dataset = 1 begin
                            WITH numbers AS (
                                SELECT cast(0x0 as uniqueidentifier) as [guid], 
                                    11 as [number],
                                    convert(nvarchar(max), concat('text-', 11)) as [text],
                                    sysdatetimeoffset() as [date]
                                UNION ALL
                                SELECT newid(),
                                    number + 1,
                                    convert(nvarchar(max),
                                    concat('text-', number + 1)),
                                    dateadd(month, -number, sysdatetimeoffset())
                                FROM numbers WHERE number < 10 + @count)
                            SELECT * FROM numbers
                            OPTION (MAXRECURSION 0);
                        end;
                        return 0;
                    end;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"exec sp_detach_db '{dbName}', 'true'";
                    cmd.ExecuteNonQuery();
                }
            }

            return new DBHelper(_connectionString);
        }
    }
}
