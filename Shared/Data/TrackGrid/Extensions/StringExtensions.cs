using System;

namespace Caerostris.Shared.Data.TrackGrid.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveContains(this string haystack, string? needle) =>
            ((needle is not null) && haystack.Contains(needle, StringComparison.OrdinalIgnoreCase));
    }
}
