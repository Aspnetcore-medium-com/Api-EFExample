using Api_EFExample.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Api_EFExample.Filters.Actions
{
    public class ResponseHeaderFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseHeaderFilter> _logger;
      
        private readonly HeaderOptions _options;
        public ResponseHeaderFilter(ILogger<ResponseHeaderFilter> logger,IOptions<HeaderOptions> options)
        {
            _logger = logger;
           _options = options.Value;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers[_options.Key] = _options.Value;
            await next();
        }
    }
}
