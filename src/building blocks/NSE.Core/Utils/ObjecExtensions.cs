
using System.Threading.Tasks;

namespace NSE.Core.Utils
{
    public static class ObjecExtensions
    {
        public static bool IsNull(this object obj) {
            var result = obj is null;
            return result;
        }

        public static bool IsNotNull(this object obj)
        {
            return !IsNull(obj);
        }

        public static async Task<bool> IsNullAsync(this object obj)
        {
            return await Task.FromResult(IsNull(obj));
        }
    }
}
