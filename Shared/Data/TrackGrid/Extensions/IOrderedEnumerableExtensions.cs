using Caerostris.Shared.Data.TrackGrid.ViewModel;
using System;
using System.Linq;

namespace Caerostris.Shared.Data.TrackGrid.Extensions
{
    public static class IOrderedEnumerableExtensions
    {
        public static IOrderedEnumerable<TSource> ThenOrderByWithDirection<TSource, TKey>(
            this IOrderedEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            SortDirection direction)
        {
            return direction switch
            {
                SortDirection.Unsorted => throw new ArgumentException("Refusing to sort unsorted"),
                SortDirection.Ascending => source.ThenBy(keySelector),
                SortDirection.Descending => source.ThenByDescending(keySelector),
                _ => throw new ArgumentException($"No such {nameof(SortDirection)}.")
            };
        }
    }
}
