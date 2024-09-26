using bone_api.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace bone_api.Middlewares
{
    namespace bone_api.Middlewares
    {
        public class ApiKeyMiddleware
        {
            private readonly RequestDelegate next;
            private readonly IConfiguration configuration;

            public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
            {
                this.configuration = configuration;
                this.next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var providedApiKey = context.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
                var isValid = checkValidationApiKey(providedApiKey);
                if (!isValid)
                {
                    await GenerateResponse(context, 401, "Invalid API Key !!");
                }
            }

            private static async Task GenerateResponse(HttpContext context, int statusCode, string errorMessage)
            {
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(errorMessage);
            }
            private bool checkValidationApiKey(string providedApiKey)
            {
                if (string.IsNullOrEmpty(providedApiKey))
                    return false;
                var validApiKey = configuration.GetValue<string>(AuthConfig.AuthSection);

                return string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal);
            }
        }
    }

}
