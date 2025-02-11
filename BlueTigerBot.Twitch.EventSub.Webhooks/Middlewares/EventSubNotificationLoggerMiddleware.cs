﻿using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591
namespace BlueTigerBot.Twitch.EventSub.Webhooks.Middlewares
{
    public class EventSubNotificationLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public EventSubNotificationLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("BlueTigerBot.Twitch.EventSub.Webhooks");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation("EventSub notification request to {CallbackPath} responded status code {StatusCode} in {ResponseTime} ms", context.Request.Path, context.Response.StatusCode, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
#pragma warning restore 1591