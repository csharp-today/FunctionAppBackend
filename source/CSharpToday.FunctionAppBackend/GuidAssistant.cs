using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CSharpToday.FunctionAppBackend
{
    public static class GuidAssistant
    {
        public static Guid ConvertToGuid(this string value) =>
            Guid.TryParse(value, out var guid) ? guid : Guid.Empty;

        public static Guid GenerateGuid(params Guid[] guids)
        {
            var key = new StringBuilder(guids.First().ToString());
            for (int i = 1; i < guids.Length; i++)
            {
                key.Append("-and-");
                key.Append(guids[i]);
            }

            return GenerateGuid(key.ToString());
        }

        public static Guid GenerateGuid(string key)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(key));
                return new Guid(hash);
            }
        }
    }
}
