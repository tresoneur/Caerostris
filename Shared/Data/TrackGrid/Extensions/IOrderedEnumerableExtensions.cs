using Caerostris.Shared.Data.TrackGrid.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Shared.Data.TrackGrid.Extensions
{
    public static class IOrderedEnumerableExtensions
    {
        public static IOrderedEnumerable<TSource> ThenOrderByWithDirection<TSource, TKey>(
            this IOrderedEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, 
            SortDirection direction)
        {
            switch (direction)
            {
                case SortDirection.Unsorted:
                    throw new ArgumentException("Refusing to sort unsorted");

                case SortDirection.Ascending:
                    return source.ThenBy(keySelector);

                case SortDirection.Descending:
                    return source.ThenByDescending(keySelector);

                default:
                    throw new ArgumentException("No such direction");
            }
        }
    }
}
