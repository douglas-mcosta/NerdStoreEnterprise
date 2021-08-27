using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Catalogo.API.Data;
using NSE.Catalogo.API.Data.Repository;
using NSE.Catalogo.API.Models;
using NSE.WebApi.Core.Usuario;

namespace NSE.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<CatalogoContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
        }
    }
}
