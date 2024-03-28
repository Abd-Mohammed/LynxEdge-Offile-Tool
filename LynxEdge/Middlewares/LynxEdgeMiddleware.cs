using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace LynxEdge.Middlewares;

public class LynxEdgeMiddleware(RequestDelegate next, ILogger<LynxEdgeMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, ex.Message);
			//context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			//var problem = new ProblemDetails
			//{
			//	Status = (int)HttpStatusCode.InternalServerError,
			//	Type = "Server Error",
			//	Title = "Server Error",
			//	Detail = "An Internal server has occurred"
			//};

			//await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
		}
	}
}