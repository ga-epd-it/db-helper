using FluentAssertions;
using System;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SPGetScalarTests
    {
        private const string spName = "SProc";

        [Fact]
        public void GetSingleValueWithNoParameter()
        {
            DBHelper DB = this.GetDBHelper();
            string result = DB.SPGetSingleValue<string>(spName);

            result.Should().Equals("null parameter");
        }

        [Fact]
        public void GetSingleValueWithSingleParameter()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "A");
            int result = DB.SPGetSingleValue<int>(spName, parameter);

            result.Should().Equals(0);
        }

        [Fact]
        public void GetStringValue()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "Z");
            string result = DB.SPGetString(spName, parameter);

            result.Should().Equals("other");
        }

        [Fact]
        public void GetStringWithImplicitConversionShouldFail()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "A");

            Action act = () => DB.SPGetString(spName, parameter);

            act.Should().Throw<InvalidCastException>()
                .WithMessage("Unable to cast object of type 'System.Int32' to type 'System.String'.");
        }

        [Fact]
        public void GetDateValue()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "C");
            DateTime result = DB.SPGetSingleValue<DateTime>(spName, parameter);

            result.Should().Equals(new DateTime(2014, 5, 1));
        }

        [Theory]
        [InlineData("D", false)]
        [InlineData("E", true)]
        public void GetBooleanValue(string testcase, bool expected)
        {
            DBHelper DB = this.GetDBHelper(testcase);
            SqlParameter parameter = new SqlParameter("@case", testcase);
            bool result = DB.SPGetBoolean(spName, parameter);

            result.Should().Equals(expected);
        }

        [Fact]
        public void GetReturnValueWithNoParameter()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter dummy = null;

            string result = DB.SPGetSingleValue<string>(spName, dummy, out int returnValue);

            result.Should().Equals("null parameter");
            returnValue.Should().Equals(0);
        }

        [Theory]
        [InlineData("D", false, 30)]
        [InlineData("E", true, 31)]
        public void GetBooleanAndReturnValue(string testcase, bool expectedResult, int expectedReturnValue)
        {
            DBHelper DB = this.GetDBHelper(testcase);
            SqlParameter parameter = new SqlParameter("@case", testcase);
            bool result = DB.SPGetBoolean(spName, parameter, out int returnValue);

            result.Should().Equals(expectedResult);
            returnValue.Should().Equals(expectedReturnValue);
        }

        [Fact]
        public void GetDateAndReturnValue()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "C");
            var result = DB.SPGetSingleValue<DateTime>(spName, parameter, out int returnValue);

            result.Should().Equals(new DateTime(2014, 5, 1));
            returnValue.Should().Equals(22);
        }

        [Fact]
        public void GetStringAndReturnValue()
        {
            DBHelper DB = this.GetDBHelper();
            SqlParameter parameter = new SqlParameter("@case", "Z");
            string result = DB.SPGetString(spName, parameter, out int returnValue);

            result.Should().Equals("other");
            returnValue.Should().Equals(99);
        }
    }
}
