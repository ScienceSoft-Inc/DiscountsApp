using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Ninject;

namespace SCNDISC.Server.Core.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void BindToMethod<T>(this IKernel config, Func<T> method)
            => config.Bind<T>().ToMethod(c => method());

        public static Type[] GetControllerTypes(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            return feature.Controllers.Select(t => t.AsType()).ToArray();
        }

        public static T GetRequestService<T>(this IApplicationBuilder builder) where T : class
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return GetRequestServiceProvider(builder).GetService<T>();
        }

        private static IServiceProvider GetRequestServiceProvider(IApplicationBuilder builder)
        {
            var accessor = builder.ApplicationServices.GetService<IHttpContextAccessor>();

            if (accessor == null)
            {
                throw new InvalidOperationException(
                    typeof(IHttpContextAccessor).FullName);
            }

            var context = accessor.HttpContext;

            if (context == null)
            {
                throw new InvalidOperationException("No HttpContext.");
            }

            return context.RequestServices;
        }
    }
}