using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System;
using System.Data.SqlClient;
using Xunit;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SqlScalarTests
    {
        [Fact]
        public void GetSingleValueWithParameterArray()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, @day1, @day2)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@day1","2014-06-05"),
                new SqlParameter("@day2","2014-08-05")
            };

            int result = DB.GetSingleValue<int>(query, parameters);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetSingleValueWithSingleParameter()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, @day1, '2014-08-05')";
            SqlParameter parameter = new SqlParameter("@day1", "2014-06-05");
            int result = DB.GetSingleValue<int>(query, parameter);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetSingleValueWithNoParameter()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";
            int result = DB.GetSingleValue<int>(query);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetIntegerWithNoParameters()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";
            int result = DB.GetInteger(query);

            result.Should().Equals(61);
        }

        [Fact]
        public void GetStringWithExplicitConversion()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select Convert(varchar(2), DATEDIFF(day, '2014-06-05', '2014-08-05'))";
            string result = DB.GetString(query);

            result.Should().Equals("61");
        }


        [Fact]
        public void GetStringWithImplicitConversionShouldFail()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select DATEDIFF(day, '2014-06-05', '2014-08-05')";

            DB.Invoking(x => x.GetString(query))
                .Should().Throw<InvalidCastException>()
                .WithMessage("Unable to cast object of type 'System.Int32' to type 'System.String'.");
        }

        [Fact]
        public void GetSingleOfDate()
        {
            DBHelper DB = this.CreateDBHelper();
            string query = "select convert(date, '2014-05-01')";
            DateTime result = DB.GetSingleValue<DateTime>(query);

            result.Should().Equals(new DateTime(2014, 5, 1));
        }
    }
}
