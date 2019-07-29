using System;

namespace EpdIt
{
    public partial class DBHelper
    {
        public class TooManyRecordsException : Exception
        {
            private const string errorMessage = "Query returned more than one record.";

            public TooManyRecordsException() : base(errorMessage) { }

            public TooManyRecordsException(string auxMessage) : base($"{errorMessage} - {auxMessage}") { }
        }
    }
}
