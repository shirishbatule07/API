
namespace Patheyam.Web.Api.Middleware
{
    using Microsoft.Extensions.Logging;
    using System;
    public static class LogMessages
    {
        private static readonly Action<ILogger, string, string, long, Exception> RoutePerformance = LoggerMessage.Define<string, string, long>(LogLevel.Information, 0,
                "{RouteName} {Method} code took {ElapsedMilliseconds}ms.");

        //static LogMessages()
        //{
        //    RoutePerformance = LoggerMessage.Define<string, string, long>(LogLevel.Information, 0,
        //        "{RouteName} {Method} code took {ElapsedMilliseconds}ms.");
        //}

        public static void LogRoutePerformance(this ILogger logger, string pageName, string method,
            long elapsedMilliseconds)
        {
            RoutePerformance(logger, pageName, method, elapsedMilliseconds, null);
        }
    }
}
