using System.Text;

namespace DataToolkit.Bootstrap.Diagnostics;

internal static class BootstrapConsole
{
    static BootstrapConsole()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    internal static void Header()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n » INICIANDO: DataToolkit Bootstrap\n\n");
        Console.ResetColor();
    }

    internal static void Registered(
        string service,
        string implementation,
        string lifetime)
    {
        Console.WriteLine($"🧩 {service} -> {implementation} ({lifetime})");
    }

    internal static void Skipped(
        string implementation,
        string reason)
    {
        Console.WriteLine($"[SKIP] {implementation} ({reason})");
    }

    internal static void Error(
        string implementation,
        Exception exception)
    {
        Console.WriteLine($"⚠️ [ERROR] {implementation}");
        Console.WriteLine($"     {exception.Message}");
    }

    internal static void Summary(
        int registered,
        int skipped,
        double nanoseconds)
    {
        Console.Write(
$"""

--------------- Summary ----------------
Registered : {registered}
Skipped    : {skipped}
Elapsed    : {FormatElapsed(nanoseconds)} ({nanoseconds:N0} ns)
----------------------------------------

""");
    }

    private static string FormatElapsed(double nanoseconds)
    {
        if (nanoseconds < 1_000)
        {
            return $"{nanoseconds:N0} ns";
        }

        if (nanoseconds < 1_000_000)
        {
            return $"{nanoseconds / 1_000:N2} µs";
        }

        if (nanoseconds < 1_000_000_000)
        {
            return $"{nanoseconds / 1_000_000:N2} ms";
        }

        return $"{nanoseconds / 1_000_000_000:N2} s";
    }
}