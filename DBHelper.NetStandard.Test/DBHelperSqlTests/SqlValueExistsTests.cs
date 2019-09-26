using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System.Data.SqlClient;
using Xunit;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SqlValueExistsTests
    {
        [Fact]
        public void ValueExistsWithParameterArrayTrue()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, @day1, @day2)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@day1","2014-06-05"),
                new SqlParameter("@day2","2014-08-05")
            };

            bool result = DB.ValueExists(query, parameters);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValueExistsWithParameterArrayFalse()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select null";
            bool result = DB.ValueExists(query);
            result.Should().BeFalse();
        }
    }
}
