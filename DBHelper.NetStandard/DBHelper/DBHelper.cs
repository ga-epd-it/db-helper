namespace EpdIt
{
    public partial class DBHelper
    {
        private readonly string _connectionString;

        public DBHelper(string connectionString) => _connectionString = connectionString;
    }
}
