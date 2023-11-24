using Entities.ErrorModel;
using Entities.Expections;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace WebApi.Extensions
{
    public static class ExpectionMiddleLayerExtensions
    {
        public static void ConfigureExpectionHandler(this WebApplication app, 
            ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";  // Content type is app/json
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();  // Catching Error Method

                    // Dynamic Error Type Cathcing
                    if (contextFeature is not null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError   // "_" mean is default
                        };

                        logger.LogError($"Something gone wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails() 
                        { 
                            StatusCode =  context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
