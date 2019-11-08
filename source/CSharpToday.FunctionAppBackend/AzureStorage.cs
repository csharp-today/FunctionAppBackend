using CSharpToday.FunctionAppBackend.Connections;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpToday.FunctionAppBackend
{
    public class AzureStorage<TModel> where TModel : StandardModel, new()
    {
        private readonly AzureStorageTable _table;

        public AzureStorage(string tableName, IAzureStorageConnectionProvider connectionProvider) =>
            _table = new AzureStorageTable(tableName, connectionProvider);

        public async Task AddAsync(TModel model) =>
            await (await _table.GetTableAsync()).ExecuteAsync(TableOperation.Insert(model));

        public async Task<TModel> GetItemAsync(Guid partitionKey, Guid rowKey)
        {
            var tableTask = _table.GetTableAsync();
            var query = new TableQuery<TModel>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(
                    nameof(StandardModel.PartitionKey),
                    QueryComparisons.Equal,
                    partitionKey.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(
                    nameof(StandardModel.RowKey),
                    QueryComparisons.Equal,
                    rowKey.ToString())
                ));

            var result = await (await tableTask).ExecuteQuerySegmentedAsync(query, null);
            if (result.Results.Count == 0)
            {
                return null;
            }

            return result.Results.First();
        }

        protected async Task<IEnumerable<TModel>> GetItemsAsync(Guid primaryKey, Func<TableQuery<TModel>, TableQuery<TModel>> queryFunc = null)
        {
            var tableTask = _table.GetTableAsync();
            var query = new TableQuery<TModel>()
                .Where(TableQuery.GenerateFilterCondition(nameof(StandardModel.PartitionKey), QueryComparisons.Equal, primaryKey.ToString()));
            if (queryFunc != null)
            {
                query = queryFunc(query);
            }

            var list = new List<TModel>();
            TableContinuationToken token = null;
            var table = await tableTask;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                list.AddRange(resultSegment.Results);
            } while (token != null);

            return list;
        }

        protected async Task ReplaceAsync(TModel model) =>
            await (await _table.GetTableAsync()).ExecuteAsync(TableOperation.Replace(model));

        protected async Task RemoveAsync(TModel model) =>
            await (await _table.GetTableAsync()).ExecuteAsync(TableOperation.Delete(model));
    }
}
