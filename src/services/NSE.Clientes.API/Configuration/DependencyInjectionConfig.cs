using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repository;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Usuario;

namespace NSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static void RegisterServices(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<ClienteContext>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            //Mediator - Command
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

            //Mediator - Event
            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
        }
    }
}
