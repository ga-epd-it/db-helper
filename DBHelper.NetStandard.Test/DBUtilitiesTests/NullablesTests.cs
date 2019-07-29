using FluentAssertions;
using System;
using Xunit;
using static EpdIt.DBUtilities;

namespace EpdIt.DBHelperTest.DBUtilitiesTests
{
    public class NullablesTests
    {
        [Fact]
        public void StringFromDbNull()
        {
            string result = GetNullableString(DBNull.Value);
            result.Should().BeNull();
        }

        [Fact]
        public void BoolFromDbNull()
        {
            bool result = GetNullable<bool>(DBNull.Value);
            result.Should().BeFalse();
        }

        [Fact]
        public void IntegerFromDbNull()
        {
            int result = GetNullable<int>(DBNull.Value);
            result.Should().Be(0);
        }

        [Fact]
        public void NullableIntegerFromDbNull()
        {
            int? result = GetNullable<int?>(DBNull.Value);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableBoolFromDbNull()
        {
            bool? result = GetNullable<bool?>(DBNull.Value);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableDateTimeFromDbNull()
        {
            DateTime? result = GetNullable<DateTime?>(DBNull.Value);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void StringFromNull()
        {
            string result = GetNullableString(null);
            result.Should().BeNull();
        }

        [Fact]
        public void BoolFromNull()
        {
            bool result = GetNullable<bool>(null);
            result.Should().BeFalse();
        }

        [Fact]
        public void IntegerFromNull()
        {
            int result = GetNullable<int>(null);
            result.Should().Be(0);
        }

        [Fact]
        public void NullableIntegerFromNull()
        {
            int? result = GetNullable<int?>(null);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableBoolFromNull()
        {
            bool? result = GetNullable<bool?>(null);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableDateTimeFromNull()
        {
            DateTime? result = GetNullable<DateTime?>(null);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void StringFromString()
        {
            string result = GetNullableString("test");
            result.Should().Be("test");
        }

        [Fact]
        public void BoolFromBool()
        {
            bool result = GetNullable<bool>(true);
            result.Should().BeTrue();
        }

        [Fact]
        public void IntFromInt()
        {
            int result = GetNullable<int>(1);
            result.Should().Be(1);
        }

        [Theory]
        [InlineData("2016-06-07")]
        [InlineData("Jun 7, 2016")]
        public void NullableDateFromValidString(string input)
        {
            DateTime? result = GetNullableDateTime(input);
            result.Should().Be(new DateTime(2016, 6, 7));
        }

        [Theory]
        [InlineData("2016-06-07 14:00:00")]
        [InlineData("Jun 7, 2016, 2:00 pm")]
        public void NullableDateTimeFromValidString(string input)
        {
            DateTime? result = GetNullableDateTime(input);
            result.Should().Be(new DateTime(2016, 6, 7, 14, 0, 0));
        }

        [Fact]
        public void NullableDateFromInvalidString()
        {
            DateTime? result = GetNullableDateTime("test");
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableDateFromBool()
        {
            DateTime? result = GetNullableDateTime(true);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }

        [Fact]
        public void NullableDateFromInt()
        {
            DateTime? result = GetNullableDateTime(2);
            result.Should().BeNull();
            result.HasValue.Should().BeFalse();
        }
    }
}
