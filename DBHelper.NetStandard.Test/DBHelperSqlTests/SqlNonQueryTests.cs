using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SqlNonQueryTests
    {
        [Fact]
        public void RunInsertWithNoParameters()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            bool result = DB.RunCommand(MultiInsertSql);

            result.Should().BeTrue();

            string query = $"select * from {TableName} where number = 1";
            DataRow row = DB.GetDataRow(query);

            row[0].Should().BeOfType<int>();
            row[0].Should().Be(1);
            row[1].Should().BeOfType<string>();
            row[1].Should().Be("text-1");
            row[2].Should().BeOfType<Guid>();
            row[2].Should().Be(default(Guid));
            row[3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)row[3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void RunInsertWithNoParametersAndCheckRowsAffected()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            SqlParameter dummy = null;
            bool result = DB.RunCommand(MultiInsertSql, dummy, out int rowsAffected);

            result.Should().BeTrue();
            rowsAffected.Should().Be(10);
        }

        [Fact]
        public void RunInsertWithOneParameters()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            var param = new SqlParameter("@count", 5);
            bool result = DB.RunCommand(MultiInsertSqlWithParam, param, out int rowsAffected);

            result.Should().BeTrue();
            rowsAffected.Should().Be(5);

            string query = $"select * from {TableName} where number = 1";
            DataRow row = DB.GetDataRow(query);

            row[0].Should().BeOfType<int>();
            row[0].Should().Be(1);
            row[1].Should().BeOfType<string>();
            row[1].Should().Be("text-1");
            row[2].Should().BeOfType<Guid>();
            row[2].Should().Be(default(Guid));
            row[3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)row[3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void RunInsertThatFails()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            string query = $"insert into {TableName} values ('abc')";

            DB.Invoking(x => x.RunCommand(query))
                .Should().Throw<SqlException>()
                .WithMessage("Column name or number of supplied values does not match table definition.");
        }

        [Fact]
        public void RunMultipleCommands()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            var queryList = new List<string>
            {
                MultiInsertSqlWithParam,
                SingleInsertSql
            };

            var paramList = new List<SqlParameter[]>
            {
                new SqlParameter[] { new SqlParameter("@count", 10) },
                null
            };

            bool result = DB.RunCommand(queryList, paramList, out List<int> rowsAffectedList);

            result.Should().BeTrue();
            rowsAffectedList[0].Should().Be(10);
            rowsAffectedList[1].Should().Be(1);

            string query = $"select * from {TableName}";
            DataTable table = DB.GetDataTable(query);
            table.Rows.Count.Should().Be(11);
            table.Rows[0][0].Should().BeOfType<int>();
            table.Rows[0][0].Should().Be(1);
            table.Rows[0][1].Should().BeOfType<string>();
            table.Rows[0][1].Should().Be("text-1");
            table.Rows[0][2].Should().BeOfType<Guid>();
            table.Rows[0][2].Should().Be(default(Guid));
            table.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)table.Rows[0][3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void RunMultipleCommandsThatFailAndTransactionRollsBack()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true);
            int initialRowCount = DB.GetInteger($"select count(*) from {TableName}");
            initialRowCount.Should().Be(0);

            var queryList = new List<string>
            {
                SingleInsertSql,
                SingleInsertSql
            };

            var paramList = new List<SqlParameter[]> { null, null };

            DB.Invoking(x => x.RunCommand(queryList, paramList))
                .Should().Throw<SqlException>()
                .WithMessage($"Violation of PRIMARY KEY constraint '{TableName}_pk'*")
                .WithMessage($"*The duplicate key value is (11).*");

            int finalRowCount = DB.GetInteger($"select count(*) from {TableName}");
            finalRowCount.Should().Be(0);
        }
    }
}
