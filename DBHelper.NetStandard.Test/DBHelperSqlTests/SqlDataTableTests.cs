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
    public class SqlDataTableTests
    {
        [Fact]
        public void GetDataTableWithNoParameters()
        {
            DBHelper DB = this.CreateDBHelper();
            DataTable result = DB.GetDataTable(TableQuery);

            result.Rows.Should().HaveCount(10);
            result.Rows[0][0].Should().BeOfType<int>();
            result.Rows[0][0].Should().Be(1);
            result.Rows[0][1].Should().BeOfType<string>();
            result.Rows[0][1].Should().Be("text-1");
            result.Rows[0][2].Should().BeOfType<Guid>();
            result.Rows[0][2].Should().Be(default(Guid));
            result.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)result.Rows[0][3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void GetDataTableWithParameter()
        {
            DBHelper DB = this.CreateDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 5);
            DataTable result = DB.GetDataTable(TableQueryWithParam, parameter);

            result.Rows.Should().HaveCount(5);
            result.Rows[0][0].Should().BeOfType<int>();
            result.Rows[0][0].Should().Be(1);
            result.Rows[0][1].Should().BeOfType<string>();
            result.Rows[0][1].Should().Be("text-1");
            result.Rows[0][2].Should().BeOfType<Guid>();
            result.Rows[0][2].Should().Be(default(Guid));
            result.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)result.Rows[0][3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void GetDataRow()
        {
            DBHelper DB = this.CreateDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 1);
            DataRow result = DB.GetDataRow(TableQueryWithParam, parameter);

            result[0].Should().BeOfType<int>();
            result[0].Should().Be(1);
            result[1].Should().BeOfType<string>();
            result[1].Should().Be("text-1");
            result[2].Should().BeOfType<Guid>();
            result[2].Should().Be(default(Guid));
            result[3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)result[3]).Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void GetLookupDictionary()
        {
            DBHelper DB = this.CreateDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 10);

            Dictionary<int, string> result = DB.GetLookupDictionary(TableQueryWithParam, parameter);

            result.Count.Should().Be(10);
            result[1].Should().Be("text-1");

            result.Invoking(x => x[0])
                .Should().Throw<KeyNotFoundException>();
        }
    }
}
