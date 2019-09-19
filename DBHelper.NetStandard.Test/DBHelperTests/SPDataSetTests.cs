using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SPGetDataSetTests
    {
        [Fact]
        public void GetSingleTableWithNoParameter()
        {
            DBHelper DB = this.GetDBHelper();
            DataTable result = DB.SPGetDataTable(spTables);

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
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 5);
            DataTable result = DB.SPGetDataTable(spTables, parameter);

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
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 1);
            DataRow result = DB.SPGetDataRow(spTables, parameter);

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
        public void GetDataRowAndReturnValue()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 1);
            DataRow result = DB.SPGetDataRow(spTables, parameter, out int returnValue);

            result[0].Should().BeOfType<int>();
            result[0].Should().Be(1);
            result[1].Should().BeOfType<string>();
            result[1].Should().Be("text-1");
            result[2].Should().BeOfType<Guid>();
            result[2].Should().Be(default(Guid));
            result[3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)result[3]).Date.Should().Be(DateTime.Today);

            returnValue.Should().Be(0);
        }

        [Fact]
        public void GetDataSet()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter[] parameters = {
                new SqlParameter("@count", 5),
                new SqlParameter("@dataset", 1)
            };
            DataSet result = DB.SPGetDataSet(spTables, parameters, out int returnValue);

            result.Should().BeOfType<DataSet>();
            result.Tables.Count.Should().Be(2);

            DataTable table0 = result.Tables[0];
            table0.Rows.Should().HaveCount(5);
            table0.Rows[0][0].Should().BeOfType<int>();
            table0.Rows[0][0].Should().Be(1);
            table0.Rows[0][1].Should().BeOfType<string>();
            table0.Rows[0][1].Should().Be("text-1");
            table0.Rows[0][2].Should().BeOfType<Guid>();
            table0.Rows[0][2].Should().Be(default(Guid));
            table0.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)table0.Rows[0][3]).Date.Should().Be(DateTime.Today);

            DataTable table1 = result.Tables[1];
            table1.Rows.Should().HaveCount(5);
            table1.Rows[0][0].Should().BeOfType<Guid>();
            table1.Rows[0][0].Should().Be(default(Guid));
            table1.Rows[0][1].Should().BeOfType<int>();
            table1.Rows[0][1].Should().Be(11);
            table1.Rows[0][2].Should().BeOfType<string>();
            table1.Rows[0][2].Should().Be("text-11");
            table1.Rows[0][3].Should().BeOfType<DateTimeOffset>();
            ((DateTimeOffset)table1.Rows[0][3]).Date.Should().Be(DateTime.Today);

            returnValue.Should().Be(1);
        }

        [Fact]
        public void GetLookupDictionary()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@count", 10);

            Dictionary<int, string> result = DB.SPGetLookupDictionary(spTables, parameter);

            result.Count.Should().Be(10);
            result[1].Should().Be("text-1");
            Assert.Throws<KeyNotFoundException>(() => result[0]);
        }
    }
}
