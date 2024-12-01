﻿using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlock.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull,IRequest<TResponse>
        where TResponse : notnull 
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
           logger.LogInformation("[start] handle request={Request} - Response={Response} - RequestData={RequestData}"
               ,typeof(TRequest).Name,typeof(TResponse).Name,request);

            var timer= new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken= timer.Elapsed;
            if(timeTaken.Seconds>3)
                logger.LogWarning("[Performance] the request {Request} took {TimeTaken}",typeof(TRequest).Name,timeTaken.Seconds);
            return response;
        }
    }
}
