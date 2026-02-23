using Microsoft.AspNetCore.Mvc.Filters;

namespace Api_EFExample.Filters.Actions
{
    public class ResponseHeaderFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseHeaderFilter> _logger;
        private readonly string Key;
        private readonly string Value;
        public ResponseHeaderFilter(ILogger<ResponseHeaderFilter> logger, string key, string value)
        {
            _logger = logger;
            Key = key;
            Value = value;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers[Key] = Value;
            await next();
        }
    }
}
