using System;

namespace CSharpToday.FunctionAppBackend
{
    public abstract class StandardModel : BaseModel
    {
        protected Guid GetPartitionKey() => PartitionKey.ConvertToGuid();
        protected Guid GetRowKey() => RowKey.ConvertToGuid();
        protected void SetPartitionKey(Guid key) => PartitionKey = key.ToString();
        protected void SetRowKey(Guid key) => RowKey = key.ToString();
    }
}
