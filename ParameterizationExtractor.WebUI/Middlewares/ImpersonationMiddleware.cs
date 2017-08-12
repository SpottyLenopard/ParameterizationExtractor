using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ParameterizationExtractor.WebUI.Middlewares
{
    public class ImpersonationMiddleware
    {
        public ImpersonationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            var winIdent = context.User.Identity as WindowsIdentity;
            if (winIdent == null)
            {
                await _next.Invoke(context);
            }
            else
            {
                using (var windowsImpersonationContext = winIdent.Impersonate())
                {
                    await _next.Invoke(context);
                }
            }
        }
    }

    public static class ImpersonationMiddlewareExtensions
    {
        public static IApplicationBuilder UseImpersonation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImpersonationMiddleware>();
        }
    }
}
