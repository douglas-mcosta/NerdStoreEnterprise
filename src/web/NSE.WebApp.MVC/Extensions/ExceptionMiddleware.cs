﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpResponseException ex)
            {

                HandleRequestExcepitionAsync(httpContext,ex);
            }
        }

        private static void HandleRequestExcepitionAsync(HttpContext context, CustomHttpResponseException httpResponseException)
        {
            if(httpResponseException.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/Login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)httpResponseException.StatusCode;
        }
    }
}