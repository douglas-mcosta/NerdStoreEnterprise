using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace NSE.WebApi.Core.Extensions
{
    #region PollyExtensions
    public static class PollyExtensions
    {

        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {

            var retry = HttpPolicyExtensions
               .HandleTransientHttpError()
               .WaitAndRetryAsync(new[]
               {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
               });

            return retry;
        }
    }
    #endregion
}
