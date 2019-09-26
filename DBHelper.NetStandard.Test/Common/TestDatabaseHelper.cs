using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EpdIt.DBHelperTest.Common
{
    internal static class TestDatabaseHelper
    {
        public const string ScalarStoredProcedure = "ScalarStoredProcedure";
        public const string DataSetStoredProcedure = "DataSetStoredProcedure";
        public const string InsertStoredProcedure = "InsertStoredProcedure";
        public const string ReturnValueStoredProcedure = "ReturnValueStoredProcedure";
        public const string OutputParameterStoredProcedure = "OutputParameterStoredProcedure";
        public const string TableName = "TestTable";

        public const string TableQuery = @"
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

        public const string TableQueryWithParam = @"
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

        public static string MultiInsertSql = $@"
            WITH numbers AS (
                SELECT 1                                          as [number],
                       convert(nvarchar(max), concat('text-', 1)) as [text],
                       cast(0x0 as uniqueidentifier)              as [guid],
                       sysdatetimeoffset()                        as [date]
                UNION ALL
                SELECT number + 1,
                       convert(nvarchar(max), concat('text-', number + 1)),
                       newid(),
                       dateadd(month, -number, sysdatetimeoffset())
                FROM numbers
                WHERE number < 10)
            insert
            into {TableName}
            SELECT *
            FROM numbers
            OPTION (MAXRECURSION 0)";

        public static string MultiInsertSqlWithParam = $@"
            WITH numbers AS (
                SELECT 1                                          as [number],
                       convert(nvarchar(max), concat('text-', 1)) as [text],
                       cast(0x0 as uniqueidentifier)              as [guid],
                       sysdatetimeoffset()                        as [date]
                UNION ALL
                SELECT number + 1,
                       convert(nvarchar(max), concat('text-', number + 1)),
                       newid(),
                       dateadd(month, -number, sysdatetimeoffset())
                FROM numbers
                WHERE number < @count)
            insert
            into {TableName}
            SELECT *
            FROM numbers
            OPTION (MAXRECURSION 0)";

        public static string SingleInsertSql = $"insert into {TableName} values (11, 'test', newid(), sysdatetimeoffset())";
        public static string SingleInsertSqlWithParam = $"insert into {TableName} values (@id, concat('test-', @id), newid(), sysdatetimeoffset())";

        public static DBHelper CreateDBHelper(
            this object callingClass,
            string appendToName = null,
            [CallerMemberName] string callingMember = null,
            bool withTable = false,
            bool withQuerySP = false,
            bool withCommandSP = false,
            bool withOutputParamSP = false
            )
        {
            string dbName = $"{callingClass.GetType().Name}_{callingMember}_{appendToName}_Test";
            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
            string dbFileName = Path.Combine(outputFolder, $"{dbName}.mdf");
            string logFileName = Path.Combine(outputFolder, $"{dbName}_log.ldf");
            string localconnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;AttachDBFilename={dbFileName}";

            Directory.CreateDirectory(outputFolder);

            using (var connection = new SqlConnection(localconnectionString))
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

            if (withTable)
            {
                CreateTable(connectionString);
            }

            if (withQuerySP)
            {
                CreateQueryStoredProcedures(connectionString);
            }

            if (withCommandSP)
            {
                CreateCommandStoredProcedures(connectionString);
            }

            if (withOutputParamSP)
            {
                CreateOutputParameterStoredProcedures(connectionString);
            }

            return new DBHelper(connectionString);
        }

        private static void CreateTable(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"IF (OBJECT_ID('dbo.{TableName}_pk') IS NOT NULL)
                BEGIN
                    ALTER TABLE {TableName} DROP CONSTRAINT {TableName}_pk
                END";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"drop table if exists {TableName}";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $@"create table {TableName} (
                number int constraint {TableName}_pk primary key nonclustered,
                text   nvarchar(max),
                guid   uniqueidentifier not null,
                date   datetimeoffset default sysdatetimeoffset() not null)";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateQueryStoredProcedures(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"create or alter procedure {ScalarStoredProcedure}
                @case varchar(1) = null as begin
                    if (@case is null) begin select 'null parameter'; return 0; end;
                    if (@case = 'A') begin select 0; return 10; end;
                    if (@case = 'B') begin select 1; return 11; end;
                    if (@case = 'C') begin select convert(date, '2014-05-01'); return 22; end;
                    if (@case = 'D') begin select convert(bit, 0); return 30; end;
                    if (@case = 'E') begin select convert(bit, 1); return 31; end;
                    select 'other'; return 99;
                end";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $@"create or alter procedure {DataSetStoredProcedure}
                @count int = 10, @dataSet bit = 0 as begin
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

                    if @dataset = 1
                        return 1;
                    return 0;
                end";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateCommandStoredProcedures(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"create or alter procedure {ReturnValueStoredProcedure}
                @case varchar(1) = null as begin
                    if (@case is null) return 0;
                    if (@case = 'A') return 10;
                    if (@case = 'B') return 11;
                    return 99;
                end";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $@"create or alter procedure {InsertStoredProcedure}
                @id int = 0 as begin
                    insert into {TableName} values (11, 'test', cast(0x0 as uniqueidentifier), sysdatetimeoffset());
                    if (@id > 0)
                        insert into {TableName} values (@id, concat('test-', @id), newid(), sysdatetimeoffset());
                    return @id;
                end";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateOutputParameterStoredProcedures(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"create or alter procedure {OutputParameterStoredProcedure}
                (@case varchar(1), @name varchar(10) OUTPUT) as begin
                    if (@case is null) select @name = 'test-null';
                    if (@case = 'A') select @name = 'test-A';
                    if (@case = 'B') select @name = 'test-B';
                    if (@case = 'C') select @name = null;
                end";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}