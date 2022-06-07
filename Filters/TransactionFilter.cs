using EntityAbstractions.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace EntityAbstractions.Persistence.Filters;

public class TransactionFilter : IAsyncActionFilter
{
    private readonly DbContext _DbContext;
    private readonly IErrorContext _notificationContext;

    public TransactionFilter(DbContext dbContext, IErrorContext notificationContext)
    {
        _DbContext = dbContext;
        _notificationContext = notificationContext;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var result = await next();
        if (!_notificationContext.HasErrors() || result.Exception == null || result.ExceptionHandled)
            await _DbContext.SaveChangesAsync();
    }
}