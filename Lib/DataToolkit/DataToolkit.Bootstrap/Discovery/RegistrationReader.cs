using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DataToolkit.Bootstrap.Discovery;

internal static class RegistrationReader
{
    private const string MethodName = "ConfigureServices";

    internal static ServiceRegistration Read(Type implementation)
    {
        var registration = new ServiceRegistration();

        var method = implementation.GetMethod(
            MethodName,
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: Type.EmptyTypes,
            modifiers: null);

        // No existe el método
        if (method is null)
            return registration;

        // Debe ser exactamente:
        // public static KeyValuePair<string,string>[] ConfigureServices()
        if (method.ReturnType != typeof(KeyValuePair<string, string>[]))
            return registration;

        if (method.GetParameters().Length != 0)
            return registration;

        KeyValuePair<string, string>[]? metadata;

        try
        {
            metadata = method.Invoke(null, null) as KeyValuePair<string, string>[];
        }
        catch
        {
            return registration;
        }

        if (metadata is null)
            return registration;

        foreach (var item in metadata)
        {
            var key = item.Key?.Trim();
            var value = item.Value?.Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                continue;

            switch (key.ToUpperInvariant())
            {
                case "LIFETIME":

                    switch (value.ToUpperInvariant())
                    {
                        case "SCOPED":
                            registration.Lifetime = ServiceLifetime.Scoped;
                            break;

                        case "SINGLETON":
                            registration.Lifetime = ServiceLifetime.Singleton;
                            break;

                        case "TRANSIENT":
                            registration.Lifetime = ServiceLifetime.Transient;
                            break;
                    }

                    break;

                case "PRIORITY":

                    if (int.TryParse(value, out var priority))
                        registration.Priority = priority;

                    break;

                case "EXCLUDE":

                    if (bool.TryParse(value, out var exclude))
                        registration.Exclude = exclude;

                    break;

                case "ASSELF":

                    if (bool.TryParse(value, out var asSelf))
                        registration.RegisterAsSelf = asSelf;

                    break;
            }
        }

        return registration;
    }
}