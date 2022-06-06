using System.Reflection;

namespace EntityAbstractions.Persistence.Contracts;

public interface IMappingAssemblyProvider
{
    Assembly Get();
}