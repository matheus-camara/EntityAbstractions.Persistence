using System.Reflection;
using EntityAbstractions.Persistence.Contracts;

namespace EntityAbstractions.Persistence.Mappings;

public class MappingAssemblyProvider : IMappingAssemblyProvider
{
    private Assembly _assembly;

    public MappingAssemblyProvider(Assembly assembly)
    {
        _assembly = assembly;
    }

    public virtual Assembly Get() => _assembly;
}