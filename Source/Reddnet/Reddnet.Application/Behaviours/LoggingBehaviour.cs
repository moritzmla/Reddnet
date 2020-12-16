using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Behaviours
{
    class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private ILogger<LoggingBehaviour<TRequest>> logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger)
            => this.logger = logger;

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"Executing Request: {request}");
            return Task.CompletedTask;
        }
    }
}
