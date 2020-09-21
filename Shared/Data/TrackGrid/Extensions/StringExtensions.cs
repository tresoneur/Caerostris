using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Shared.Data.TrackGrid.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveContains(this string haystack, string? needle) // TODO: ékezetek etc
        {
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0; // TODO: culture
        }
    }
}
