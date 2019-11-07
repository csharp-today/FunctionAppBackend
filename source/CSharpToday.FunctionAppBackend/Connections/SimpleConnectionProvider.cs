namespace CSharpToday.FunctionAppBackend.Connections
{
    public class SimpleConnectionProvider : IAzureStorageConnectionProvider
    {
        private readonly string _connectionString;

        public SimpleConnectionProvider(string connectionString) => _connectionString = connectionString;

        public string GetAzureStorageConnection() => _connectionString;
    }
}
