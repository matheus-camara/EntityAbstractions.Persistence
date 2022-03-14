using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EntityAbstractions.Persistence.Generators;

public class DateGenerator : ValueGenerator<DateTime?>
{
    public override bool GeneratesTemporaryValues => false;

    public override DateTime? Next(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }
}