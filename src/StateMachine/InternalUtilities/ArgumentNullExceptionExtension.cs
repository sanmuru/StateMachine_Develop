using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

internal static class ArgumentNullExceptionExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull([NotNull] this object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NETSTANDARD2_0
        if (argument is null) throw new ArgumentNullException(nameof(argument));
#else
        ArgumentNullException.ThrowIfNull(argument, paramName);
#endif
    }
}
