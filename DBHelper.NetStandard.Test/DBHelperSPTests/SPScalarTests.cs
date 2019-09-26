using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SPScalarTests
    {
        [Fact]
        public void GetSingleValueWithNoParameter()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            string result = DB.SPGetSingleValue<string>(ScalarStoredProcedure);

            result.Should().Equals("null parameter");
        }

        [Fact]
        public void GetSingleValueWithSingleParameter()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "A");
            int result = DB.SPGetSingleValue<int>(ScalarStoredProcedure, parameter);

            result.Should().Equals(0);
        }

        [Fact]
        public void GetStringValue()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "Z");
            string result = DB.SPGetString(ScalarStoredProcedure, parameter);

            result.Should().Equals("other");
        }

        [Fact]
        public void GetStringWithImplicitConversionShouldFail()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "A");

            DB.Invoking(x => x.SPGetString(ScalarStoredProcedure, parameter))
                .Should().Throw<InvalidCastException>()
                .WithMessage("Unable to cast object of type 'System.Int32' to type 'System.String'.");
        }

        [Fact]
        public void GetDateValue()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "C");
            DateTime result = DB.SPGetSingleValue<DateTime>(ScalarStoredProcedure, parameter);

            result.Should().Equals(new DateTime(2014, 5, 1));
        }

        [Theory]
        [InlineData("D", false)]
        [InlineData("E", true)]
        public void GetBooleanValue(string testCase, bool expected)
        {
            DBHelper DB = this.CreateDBHelper(appendToName: testCase, withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", testCase);
            bool result = DB.SPGetBoolean(ScalarStoredProcedure, parameter);

            result.Should().Equals(expected);
        }

        [Fact]
        public void GetReturnValueWithNoParameter()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter dummy = null;

            string result = DB.SPGetSingleValue<string>(ScalarStoredProcedure, dummy, out int returnValue);

            result.Should().Equals("null parameter");
            returnValue.Should().Equals(0);
        }

        [Theory]
        [InlineData("D", false, 30)]
        [InlineData("E", true, 31)]
        public void GetBooleanAndReturnValue(string testCase, bool expectedResult, int expectedReturnValue)
        {
            DBHelper DB = this.CreateDBHelper(appendToName: testCase, withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", testCase);
            bool result = DB.SPGetBoolean(ScalarStoredProcedure, parameter, out int returnValue);

            result.Should().Equals(expectedResult);
            returnValue.Should().Equals(expectedReturnValue);
        }

        [Fact]
        public void GetDateAndReturnValue()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "C");
            var result = DB.SPGetSingleValue<DateTime>(ScalarStoredProcedure, parameter, out int returnValue);

            result.Should().Equals(new DateTime(2014, 5, 1));
            returnValue.Should().Equals(22);
        }

        [Fact]
        public void GetStringAndReturnValue()
        {
            DBHelper DB = this.CreateDBHelper(withQuerySP: true);
            SqlParameter parameter = new SqlParameter("@case", "Z");
            string result = DB.SPGetString(ScalarStoredProcedure, parameter, out int returnValue);

            result.Should().Equals("other");
            returnValue.Should().Equals(99);
        }
    }
}
