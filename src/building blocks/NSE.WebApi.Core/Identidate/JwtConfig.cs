using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtExtensions;
using System.Net.Http;
using System.Text;

namespace NSE.WebApi.Core.Identidate
{
    public static class JwtConfig
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            //JWT
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<Token>(appSettingsSection);

            var appSettings = appSettingsSection.Get<Token>();

            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(bearerOptions =>
            {

                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                bearerOptions.SaveToken = true;
                bearerOptions.SetJwksOptions(new JwkOptions(appSettings.AutenticacaoJwksUrl));
            });

            return services;
        }

        public static IApplicationBuilder UseJwtConfiguration(this IApplicationBuilder app)
        {

            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
