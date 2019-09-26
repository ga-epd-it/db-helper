using EpdIt.DBHelperTest.Common;
using FluentAssertions;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using static EpdIt.DBHelperTest.Common.TestDatabaseHelper;
using static EpdIt.DBUtilities;

namespace EpdIt.DBHelperTest.DBHelperTests
{
    public class SPOutputParameterTests
    {
        [Theory]
        [InlineData(null, "test-null")]
        [InlineData("A", "test-A")]
        [InlineData("B", "test-B")]
        [InlineData("C", null)]
        public void RunSPWithOutputParameter(string testCase, string expectedResult)
        {
            DBHelper DB = this.CreateDBHelper(appendToName: testCase, withTable: true, withOutputParamSP: true);

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@case", testCase),
                new SqlParameter("@name", SqlDbType.VarChar)
                {
                    Direction = ParameterDirection.Output,
                    Size = 10
                }
            };

            bool success = DB.SPRunCommand(OutputParameterStoredProcedure, parameters);

            success.Should().BeTrue();
            string result = GetNullableString(parameters[1].Value);
            result.Should().Be(expectedResult);
        }
    }
}
