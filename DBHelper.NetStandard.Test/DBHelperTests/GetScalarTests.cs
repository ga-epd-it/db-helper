using FluentAssertions;
using System;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class GetScalarTests
    {
        [Fact]
        public void GetSingleValueWithParameterArray()
        {
            string query = "select DATEDIFF(day, @day1, @day2)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@day1","2014-06-05"),
                new SqlParameter("@day2","2014-08-05")
            };

            DBHelper DB = this.GetDBHelper();
            int result = DB.GetSingleValue<int>(query, parameters);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetSingleValueWithSingleParameter()
        {
            string query = "select DATEDIFF(day, @day1, '2014-08-05')";
            SqlParameter parameter = new SqlParameter("@day1", "2014-06-05");

            DBHelper DB = this.GetDBHelper();
            int result = DB.GetSingleValue<int>(query, parameter);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetSingleValueWithNoParameter()
        {
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";

            DBHelper DB = this.GetDBHelper();
            int result = DB.GetSingleValue<int>(query);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetIntegerWithNoParameters()
        {
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";

            DBHelper DB = this.GetDBHelper();
            int result = DB.GetInteger(query);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetStringWithExplicitConversion()
        {
            string query = "select Convert(varchar(2), DATEDIFF(day, '2014-06-05', '2014-08-05'))";

            DBHelper DB = this.GetDBHelper();
            string result = DB.GetString(query);

            result.Should().Equals("61");
        }


        [Fact]
        public void GetStringWithImplicitConversionShouldFail()
        {
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";

            DBHelper DB = this.GetDBHelper();

            Action act = () => DB.GetString(query);

            act.Should().Throw<InvalidCastException>()
                .WithMessage("Unable to cast object of type 'System.Int32' to type 'System.String'.");
        }

        [Fact]
        public void GetSingleOfDate()
        {
            string query = "select convert(date, '2014-05-01')";

            DBHelper DB = this.GetDBHelper();
            DateTime result = DB.GetSingleValue<DateTime>(query);

            result.Should().Equals(new DateTime(2014,5,1));
        }

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
