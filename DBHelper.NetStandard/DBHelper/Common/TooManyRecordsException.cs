using System;

namespace EpdIt
{
    public partial class DBHelper
    {
        public class TooManyRecordsException : Exception
        {
            private const string _errorMessage = "Query returned more than one record.";

            public TooManyRecordsException() : base(_errorMessage) { }

            public TooManyRecordsException(string auxMessage) : base($"{_errorMessage} - {auxMessage}") { }
        }
    }
}
