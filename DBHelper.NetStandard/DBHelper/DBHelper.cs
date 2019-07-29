namespace EpdIt
{
    public partial class DBHelper
    {
        private readonly string connectionString;

        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
