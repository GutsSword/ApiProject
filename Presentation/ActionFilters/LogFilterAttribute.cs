using Entities.LogModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Contracts;
using System.Text.Json.Serialization;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILoggerService _logger;

        public LogFilterAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        private string Log(string modelName, Microsoft.AspNetCore.Routing.RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"]
            };

            if (routeData.Values.Count >= 3)   // İf >3 There exist ID.
                logDetails.Id = routeData.Values["Id"];

            return logDetails.ToString();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }
        
    }
}
