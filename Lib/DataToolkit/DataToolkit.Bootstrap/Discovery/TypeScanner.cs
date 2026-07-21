using System.Reflection;

namespace DataToolkit.Bootstrap.Discovery;

internal static class TypeScanner
{
    internal static IReadOnlyCollection<CandidateType> Scan(
        IEnumerable<BootstrapModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        List<CandidateType> result = [];

        foreach (BootstrapModule module in modules)
        {
            foreach (Type type in GetLoadableTypes(module.Assembly))
            {
                if (!MatchesNamespace(type, module))
                {
                    continue;
                }

                if (!IsCandidate(type))
                {
                    continue;
                }

                ServiceRegistration registration =
                    RegistrationReader.Read(type);

                if (registration.Exclude)
                {
                    continue;
                }

                Type[] interfaces = GetPublicInterfaces(type);

                result.Add(new CandidateType(
                    type,
                    interfaces,
                    registration));
            }
        }

        return result;
    }

    private static Type[] GetPublicInterfaces(Type type)
    {
        Type[] interfaces = type.GetInterfaces();

        int count = 0;

        for (int i = 0; i < interfaces.Length; i++)
        {
            if (interfaces[i].IsPublic)
            {
                count++;
            }
        }

        if (count == interfaces.Length)
        {
            return interfaces;
        }

        Type[] result = new Type[count];

        int index = 0;

        for (int i = 0; i < interfaces.Length; i++)
        {
            if (interfaces[i].IsPublic)
            {
                result[index++] = interfaces[i];
            }
        }

        return result;
    }

    private static Type[] GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            Type?[] source = ex.Types;

            int count = 0;

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] is not null)
                {
                    count++;
                }
            }

            Type[] result = new Type[count];

            int index = 0;

            for (int i = 0; i < source.Length; i++)
            {
                Type? type = source[i];

                if (type is not null)
                {
                    result[index++] = type;
                }
            }

            return result;
        }
    }

    private static bool IsCandidate(Type type)
    {
        return type.IsClass
            && !type.IsAbstract
            && type.IsPublic
            && !type.IsNested
            && !type.IsGenericTypeDefinition;
    }

    private static bool MatchesNamespace(
        Type type,
        BootstrapModule module)
    {
        string? ns = type.Namespace;

        if (ns is null)
        {
            return false;
        }

        if (ns != module.RootNamespace &&
            !ns.StartsWith(module.RootNamespace + ".", StringComparison.Ordinal))
        {
            return false;
        }

        ReadOnlySpan<char> span = ns.AsSpan();

        int lastSeparator = span.LastIndexOf('.');

        ReadOnlySpan<char> lastSegment =
            lastSeparator >= 0
                ? span[(lastSeparator + 1)..]
                : span;

        return lastSegment.Equals(
            module.TargetNamespace,
            StringComparison.Ordinal);
    }
}