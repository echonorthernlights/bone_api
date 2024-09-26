using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace bone_api.Authentication;
public class ApiKeyAuthenticationFilter : IAsyncAuthorizationFilter
{
    private readonly IConfiguration configuration;

    public ApiKeyAuthenticationFilter(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var providedApiKey = context.HttpContext.Request.Headers[AuthConfig.ApiKeyHeader];
        var isValid = checkValidationApiKey(providedApiKey);
        if (!isValid)
        {
            context.Result = new UnauthorizedObjectResult("Invaliiiid authentication!!");
            return;
        }



    }

    private bool checkValidationApiKey(string providedApiKey)
    {
        if (string.IsNullOrEmpty(providedApiKey))
            return false;
        var validApiKey = configuration.GetValue<string>(AuthConfig.AuthSection);

        return string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal);
    }
}