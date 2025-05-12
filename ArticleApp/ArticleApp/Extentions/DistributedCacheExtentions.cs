using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ArticleApp.Extentions
{
    public static class DistributedCacheExtentions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId,
            T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            //// set the expired time to this cache (parameter or default)
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            /// set where this cache will end if it is not used 
            options.SlidingExpiration = unusedExpireTime;

            /// serlize the data to json
            var jsonData = JsonSerializer.Serialize(data);
            /// SET THE RECORD IN THE DATABASE 
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
