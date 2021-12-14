using MediatR;
using Microsoft.Extensions.Logging;
using Reddnet.Application.Validation;

namespace Reddnet.Application.Behaviours;

class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private ILogger<LoggingBehaviour<TRequest, TResponse>> logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => this.logger = logger;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        this.logger.LogInformation($"Request: {request}");
        var response = await next();

        if (response is Result result && result.IsError)
        {
            this.logger.LogError($"Response: {response}");
        }
        else
        {
            this.logger.LogInformation($"Response: {response}");
        }

        return response;
    }
}
