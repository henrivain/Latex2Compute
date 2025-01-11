using System.Diagnostics;
using System.Reflection;

namespace Latex2Compute.Helpers;
internal static class AssemblyInfoHelper
{
    internal static class EntryAssembly
    {
        internal static bool? IsDebug { get; } = IsAssemblyInDebugMode(Assembly.GetEntryAssembly());
    }

    internal static class CurrentAssembly
    {
        internal static bool? IsDebug { get; } = IsAssemblyInDebugMode(typeof(CurrentAssembly).Assembly);
    }

    private static bool IsAssemblyInDebugMode(Assembly? assembly) => assembly?.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled ?? false;
}
