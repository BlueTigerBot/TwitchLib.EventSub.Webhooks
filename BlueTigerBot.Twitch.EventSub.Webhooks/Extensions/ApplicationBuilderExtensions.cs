using System;
using System.Net.Http;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Extensions
{
    /// <summary>
    /// Extension methods for ApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds needed Middlewares for BlueTigerBot.Twitch.EventSub.Webhooks to the specified IApplicationBuilder
        /// </summary>
        /// <param name="app">IApplicationBuilder used my .NET to build the execution pipeline</param>
        /// <returns>Reference to IApplicationBuilder after the operation has completed</returns>
        public static IApplicationBuilder UseTwitchLibEventSubWebhooks(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<TwitchLibEventSubOptions>>();

            app.UseWhen(context => options.Value.EnableLogging &&
                                context.Request.Method.Equals(HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase) &&
                                context.Request.Path.Equals(options.Value.CallbackPath, StringComparison.InvariantCultureIgnoreCase), appBuilder =>
            {
                appBuilder.UseMiddleware<EventSubNotificationLoggerMiddleware>();
            });

            app.UseWhen(context => context.Request.Method.Equals(HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase)
                                   && context.Request.Path.Equals(options.Value.CallbackPath, StringComparison.InvariantCultureIgnoreCase), appBuilder =>
            {
                appBuilder.UseMiddleware<EventSubSignatureVerificationMiddleware>();
                appBuilder.UseMiddleware<EventSubNotificationMiddleware>();
            });

            return app;
        }
    }
}