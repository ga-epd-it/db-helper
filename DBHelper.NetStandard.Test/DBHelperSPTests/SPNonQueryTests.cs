using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SPNonQueryTests
    {
        [Theory]
        [InlineData(null, 0)]
        [InlineData("A", 10)]
        [InlineData("B", 11)]
        [InlineData("C", 99)]
        public void RunSPWithOnlyReturnValue(string testCase, int expectedResult)
        {
            DBHelper DB = this.CreateDBHelper(appendToName: testCase, withTable: true, withCommandSP: true);
            var parameter = new SqlParameter("@case", testCase);
            int returnValue = DB.SPReturnValue(ReturnValueStoredProcedure, parameter, out int rowsAffected);

            returnValue.Should().Be(expectedResult);
            rowsAffected.Should().Be(-1);
        }

        [Fact]
        public void RunSPInsertWithNoParameters()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true, withCommandSP: true);
            bool result = DB.SPRunCommand(InsertStoredProcedure);

            result.Should().BeTrue();

            string query = $"select * from {TableName}";
            DataTable table = DB.GetDataTable(query);

            table.Rows.Count.Should().Be(1);
            table.Rows[0][0].Should().BeOfType<int>();
            table.Rows[0][0].Should().Be(11);
            table.Rows[0][1].Should().BeOfType<string>();
            table.Rows[0][1].Should().Be("test");
            table.Rows[0][2].Should().BeOfType<Guid>();
            table.Rows[0][2].Should().Be(default(Guid));
            table.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)table.Rows[0][3]).Date.Should().Be(DateTime.Today);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(13)]
        public void RunSPInsertWithOneParameterAndReturnValue(int id)
        {
            DBHelper DB = this.CreateDBHelper(appendToName: id.ToString(), withTable: true, withCommandSP: true);
            var param = new SqlParameter("@id", id);
            bool result = DB.SPRunCommand(InsertStoredProcedure, param, out int rowsAffected, out int returnValue);

            result.Should().BeFalse();
            rowsAffected.Should().Be(2);
            returnValue.Should().Be(id);

            string query = $"select * from {TableName}";
            DataTable table = DB.GetDataTable(query);

            table.Rows.Count.Should().Be(2);
            table.Rows[1][0].Should().BeOfType<int>();
            table.Rows[1][0].Should().Be(id);
            table.Rows[1][1].Should().BeOfType<string>();
            table.Rows[1][1].Should().Be("test-" + id);
            table.Rows[1][2].Should().BeOfType<Guid>();
            table.Rows[1][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)table.Rows[0][3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void RunSPThatFails()
        {
            DBHelper DB = this.CreateDBHelper(withTable: true, withCommandSP: true);
            var param = new SqlParameter("@id", 11);
            var ex = DB.Invoking(
                x => x.SPRunCommand(InsertStoredProcedure, param, out _, out _))
                .Should().Throw<SqlException>()
                .WithMessage($"Violation of PRIMARY KEY constraint '{TableName}_pk'*")
                .WithMessage($"*The duplicate key value is (11).*");
        }
    }
}
