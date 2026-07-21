using System.Reflection;

namespace DataToolkit.Bootstrap;


public sealed class BootstrapOptions
{
    /// <summary>
    /// Ensamblados que serán escaneados.
    /// </summary>
    public IList<Assembly> Assemblies { get; } = [];

    /// <summary>
    /// Carpeta donde buscar ensamblados (*.dll).
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Patrón de búsqueda de ensamblados.
    /// </summary>
    public string SearchPattern { get; set; } = "*.dll";

    /// <summary>
    /// Mostrar información del proceso de descubrimiento.
    /// </summary>
    public bool Diagnostics { get; set; } = true;
}
