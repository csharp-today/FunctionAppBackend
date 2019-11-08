using Microsoft.Azure.Cosmos.Table;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CSharpToday.FunctionAppBackend
{
    public abstract class StandardModel : TableEntity
    {
        protected static Guid GenerateGuid(params Guid[] guids)
        {
            var key = new StringBuilder(guids.First().ToString());
            for (int i = 1; i < guids.Length; i++)
            {
                key.Append("-and-");
                key.Append(guids[i]);
            }

            return GenerateGuid(key.ToString());
        }

        protected static Guid GenerateGuid(string key)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(key));
                return new Guid(hash);
            }
        }

        protected Guid GetPartitionKey() => Guid.TryParse(PartitionKey, out var guid) ? guid : Guid.Empty;
        protected Guid GetRowKey() => Guid.TryParse(RowKey, out var guid) ? guid : Guid.Empty;
        protected void SetPartitionKey(Guid key) => PartitionKey = key.ToString();
        protected void SetRowKey(Guid key) => RowKey = key.ToString();
    }
}
