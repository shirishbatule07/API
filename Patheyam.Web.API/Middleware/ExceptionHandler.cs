
namespace Patheyam.Web.Api.Middleware
{
    using Patheyam.Common;
    using Patheyam.Web.Api.Utils;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Threading.Tasks;

    public sealed class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionType = exception.GetType();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            if (exceptionType == typeof(SqlException) && ((SqlException)exception).State == 0 && ((SqlException)exception).Class == 16)
            {
                var envelope = Envelope.Error(exception.Message);
                _logger.LogError($"Sql Bad Request Exception : {exception.Message}");
                var result = JsonConvert.SerializeObject(envelope, jsonSerializerSettings);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(result);
            }
            else if (exceptionType == typeof(ValidationException))
            {
                var envelope = Envelope.Error(exception.Message);
                var result = JsonConvert.SerializeObject(envelope, jsonSerializerSettings);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(result);
            }
            else
            {
                var error = new ApiError
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = (short)HttpStatusCode.InternalServerError,
                    Title = "Some kind of error occurred in the API.  Please use the id and contact our " +
                            "support team if the problem persists."
                };

                var innerExMessage = GetInnermostExceptionMessage(exception);

                _logger.Log(LogLevel.Error, exception, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", error.Id);

                var result = JsonConvert.SerializeObject(error, jsonSerializerSettings);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(result);
            }
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnermostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}
