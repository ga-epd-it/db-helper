using FluentAssertions;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;
using static EpdIt.DBUtilities;

namespace EpdIt.DBHelperTest.DBUtilitiesTests
{
    public class TvpTests
    {
        [Fact]
        public void CreateIntegerTvp()
        {
            int[] values = { 1, 2 };
            SqlParameter tvp = TvpSqlParameter("parameterName", values, "dbTableTypeName", "dbColumnName");

            IEnumerable<SqlDataRecord> records = (IEnumerable<SqlDataRecord>)tvp.Value;

            tvp.SqlDbType.Should().Be(SqlDbType.Structured);
            tvp.SqlValue.Should().BeNull();

            records.Count().Should().Be(values.Count());
            records.ToList()[0].FieldCount.Should().Be(1);
            records.ToList()[0].GetSqlFieldType(0).Should().Be(typeof(System.Data.SqlTypes.SqlInt32));
            records.ToList()[0].GetValue(0).Should().Be(values[0]);
            records.ToList()[1].GetValue(0).Should().Be(values[1]);
        }

        [Fact]
        public void CreateStringTvp()
        {
            string[] values = { "A", "B" };
            SqlParameter tvp = TvpSqlParameter("parameterName", values, "dbTableTypeName", "dbColumnName");

            IEnumerable<SqlDataRecord> records = (IEnumerable<SqlDataRecord>)tvp.Value;

            tvp.SqlDbType.Should().Be(SqlDbType.Structured);
            tvp.SqlValue.Should().BeNull();

            records.Count().Should().Be(values.Count());
            records.ToList()[0].FieldCount.Should().Be(1);
            records.ToList()[0].GetSqlFieldType(0).Should().Be(typeof(System.Data.SqlTypes.SqlString));
            records.ToList()[0].GetValue(0).Should().Be(values[0]);
            records.ToList()[1].GetValue(0).Should().Be(values[1]);
        }
    }
}
