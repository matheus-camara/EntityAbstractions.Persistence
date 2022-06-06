using EntityAbstractions.Persistence.Generators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityAbstractions.Persistence.Mappings;

public abstract class AuditableEntityMapping<T> : TrackableMapping<T> where T : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Created).HasValueGenerator<DateGenerator>().ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.Modified).HasValueGenerator<DateGenerator>().ValueGeneratedOnAddOrUpdate();
        builder.Property(x => x.ModifiedBy);
    }
}