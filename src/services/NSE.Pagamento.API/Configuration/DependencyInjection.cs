﻿
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Pagamentos.API.Data;
using NSE.Pagamentos.API.Data.Repository;
using NSE.Pagamentos.API.Facede;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.API.Services;
using NSE.WebApi.Core.Usuario;

namespace NSE.Pagamentos.API.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<PagamentosContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPagamentoService,PagamentoService>(); 
            services.AddScoped<IPagamentoFacede,PagamentoCartaoCreditoFacede>(); 

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }
    }
}
