using System;
using System.Threading.Tasks;
using Google.Apis.Util.Store;
using Utility.Serialization;

namespace GMaster.Common.Google
{
    public class SqlDataStore : IDataStore
    {
        public SqlDataStore()
        {
        }

        public async Task ClearAsync()
        {
            Query.GoogleTokens.Clear();
        }

        public async Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be empty");
            }
            Query.GoogleTokens.Delete(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be empty");
            }
            var value = Query.GoogleTokens.Get(key);
            return Serializer.ReadObject<T>(value);
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be empty");
            }
            Query.GoogleTokens.Create(key, Serializer.WriteObjectToString(value));
        }
    }
}
