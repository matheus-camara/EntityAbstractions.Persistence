using System.Reflection;
using EntityAbstractions.Persistence.Contexts;
using EntityAbstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityAbstractions.Persistence.Configuration;

public static class DependancyInjection
{
    public static void AddRepositories(this IServiceCollection services, Assembly[] assemblies,
        IConfiguration configuration)
    {
        services.AddDbContext<Context>(options => options.UseSqlServer(configuration.GetConnectionString("Main")));
        services.AddGenericRepositories(assemblies);
    }

    private static IEnumerable<Type> GetTypesOfImplementations(Assembly[] assemblies, Type baseType)
    {
        return assemblies.SelectMany(a => a.DefinedTypes.Where(x =>
            !x.IsAbstract
            && !x.IsInterface
            && x.BaseType is not null
            && x.BaseType.IsGenericType
            && x.BaseType.GetGenericTypeDefinition() == baseType)
        ).ToList();
    }

    private static void AddGenericRepositories(this IServiceCollection services, Assembly[] assemblies)
    {
        var baseType = typeof(Repository<>);
        var types = GetTypesOfImplementations(assemblies, baseType);
        foreach (var type in types)
        {
            var typeMostSpecificInterface = type.GetInterfaces().First(x => !x.IsAbstract && !x.IsGenericType);
            services.Add(new ServiceDescriptor(typeMostSpecificInterface, type, ServiceLifetime.Scoped));
        }
    }
}