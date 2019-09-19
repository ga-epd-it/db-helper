using FluentAssertions;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SqlValueExistsTests
    {
        [Fact]
        public void ValueExistsWithParameterArrayTrue()
        {
            string query = "select DATEDIFF(day, @day1, @day2)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@day1","2014-06-05"),
                new SqlParameter("@day2","2014-08-05")
            };

            DBHelper DB = this.GetDBHelper();

            bool result = DB.ValueExists(query, parameters);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValueExistsWithParameterArrayFalse()
        {
            string query = "select null";

            DBHelper DB = this.GetDBHelper();

            bool result = DB.ValueExists(query);

            result.Should().BeFalse();
        }
    }
}
