using System.Reflection;

namespace DataToolkit.Bootstrap.Discovery;

internal readonly record struct BootstrapModule(
    Assembly Assembly,
    string RootNamespace,
    string TargetNamespace);
