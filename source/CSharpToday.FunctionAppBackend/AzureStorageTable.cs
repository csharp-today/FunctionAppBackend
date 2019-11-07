using CSharpToday.FunctionAppBackend.Connections;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace CSharpToday.FunctionAppBackend
{
    public class AzureStorageTable
    {
        private readonly IAzureStorageConnectionProvider _connectionProvider;

        private CloudTable _table;
        private object _tableLock = new object();

        public string TableName { get; }

        public AzureStorageTable(string tableName, IAzureStorageConnectionProvider connectionProvider) =>
            (TableName, _connectionProvider) = (tableName, connectionProvider);

        public async Task<CloudTable> GetTableAsync()
        {
            if (_table != null)
            {
                return _table;
            }

            var table = CloudStorageAccount
                .Parse(_connectionProvider.GetAzureStorageConnection())
                .CreateCloudTableClient()
                .GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();

            lock (_tableLock)
            {
                if (_table is null)
                {
                    _table = table;
                }
            }

            return _table;
        }
    }
}
