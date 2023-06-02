using System.Diagnostics;
using MediatR;

namespace OrderService.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        _logger.LogInformation("Executing request: {RequestName}", typeof(TRequest).Name);

        var response = await next();

        stopwatch.Stop();

        _logger.LogInformation("Request executed: {RequestName}. Execution time: {ElapsedTime}ms",
            typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

        return response;
    }
}