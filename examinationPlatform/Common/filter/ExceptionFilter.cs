using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Common.filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            var actionName = context.HttpContext.Request.RouteValues["controller"] + "/" + context.HttpContext.Request.RouteValues["action"];
            _logger.LogError($"--------{actionName} Error Begin--------");
            _logger.LogError($"  Error Detail:" + context.Exception.Message);
            if (!context.ExceptionHandled)
            {
                context.ExceptionHandled = true;
                var result = new RedirectResult("/html/error.html");
                context.Result = result;
            }            
            _logger.LogError($"--------{actionName} Error end--------");
        }
    }
}
