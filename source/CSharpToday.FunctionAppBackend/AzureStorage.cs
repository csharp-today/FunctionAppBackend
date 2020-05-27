using CSharpToday.FunctionAppBackend.Connections;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpToday.FunctionAppBackend
{
    public class AzureStorage<TModel> where TModel : StandardModel, new()
    {
        private readonly BaseAzureStorage<TModel> _baseStorage;
        public AzureStorage(string tableName, IAzureStorageConnectionProvider connectionProvider) =>
            _baseStorage = new BaseAzureStorage<TModel>(tableName, connectionProvider);

        public Task AddAsync(TModel model) => _baseStorage.AddAsync(model);

        public Task<TModel> GetAsync(Guid partitionKey, Guid rowKey) =>
            _baseStorage.GetAsync(partitionKey.ToString(), rowKey.ToString());

        protected Task<IEnumerable<TModel>> GetItemsAsync(Guid primaryKey, Func<TableQuery<TModel>, TableQuery<TModel>> queryFunc = null) =>
            _baseStorage.GetItemsAsync(primaryKey.ToString(), queryFunc);

        protected Task ReplaceAsync(TModel model) => _baseStorage.ReplaceAsync(model);

        protected Task RemoveAsync(TModel model) => _baseStorage.RemoveAsync(model);
    }
}
