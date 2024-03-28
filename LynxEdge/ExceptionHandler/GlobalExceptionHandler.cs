using Microsoft.AspNetCore.Mvc;

namespace LynxEdge.ExceptionHandler;

public class GlobalExceptionHandler(
	RequestDelegate next,
	ILogger<GlobalExceptionHandler> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<GlobalExceptionHandler> _logger = logger;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception exception)
		{
			_logger.LogError(
				exception, "Exception occurred: {Message}", exception.Message);

			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Server Error", 
				Detail = exception.Message
			};

			context.Response.StatusCode =
				StatusCodes.Status500InternalServerError;

			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}