using EntityAbstractions.Persistence.Contexts;
using EntityAbstractions.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntityAbstractions.Persistence.Filters;

public class TransactionFilter : IAsyncActionFilter
{
    private readonly Context _DbContext;
    private readonly IErrorContext _notificationContext;

    public TransactionFilter(Context dbContext, IErrorContext notificationContext)
    {
        _DbContext = dbContext;
        _notificationContext = notificationContext;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var result = await next();
        if (_notificationContext.HasErrors() || result.Exception == null || result.ExceptionHandled)
            await _DbContext.SaveChangesAsync();
    }
}