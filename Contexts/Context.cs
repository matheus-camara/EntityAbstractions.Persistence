using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EntityAbstractions.Persistence.Contexts;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(Context))!, x => !x.IsAbstract);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
            if (entry is { State: EntityState.Added or EntityState.Modified, Entity: AuditableEntity auditable })
                Entry(auditable).Property(x => x.Modified).CurrentValue = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }
}