using System.Reflection;

namespace DataToolkit.Bootstrap.Discovery;

internal static class AssemblyLoader
{
    internal static IReadOnlyCollection<Assembly> Load(string folder)
    {
        return Load(folder, "*.dll");
    }

    internal static IReadOnlyCollection<Assembly> Load(
        string folder,
        string searchPattern)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException(
                "The folder cannot be null or empty.",
                nameof(folder));

        if (!Directory.Exists(folder))
            throw new DirectoryNotFoundException(folder);

        var assemblies = new List<Assembly>();

        foreach (var file in Directory.EnumerateFiles(
                     folder,
                     searchPattern,
                     SearchOption.TopDirectoryOnly))
        {
            try
            {
                assemblies.Add(Assembly.LoadFrom(file));
            }
            catch
            {
                // Ignorar DLLs que no sean .NET o que no puedan cargarse.
            }
        }

        return assemblies;
    }
}