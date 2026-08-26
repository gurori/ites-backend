using ites.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ites.Server.Filters;

public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        await Task.Run(() =>
        {
            if (context.Exception is ApiException apiEx)
            {
                context.Result = new ObjectResult(new { detail = apiEx.Message })
                {
                    StatusCode = apiEx.StatusCode,
                };

                context.ExceptionHandled = true;
            }
        });
    }
}
