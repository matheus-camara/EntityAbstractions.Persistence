using EntityAbstractions.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EntityAbstractions.Persistence.Contexts;

public class Context : DbContext
{
    private readonly IMappingAssemblyProvider _assemblyProvider;

    public Context(DbContextOptions options, IMappingAssemblyProvider assemblyProvider) : base(options)
    {
        _assemblyProvider = assemblyProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_assemblyProvider.Get(), x => !x.IsAbstract);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
            if (entry is { State: EntityState.Added or EntityState.Modified, Entity: AuditableEntity auditable })
                Entry(auditable).Property(x => x.Modified).CurrentValue = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }
}