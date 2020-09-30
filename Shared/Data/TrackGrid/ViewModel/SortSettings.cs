using System;

namespace Caerostris.Shared.Data.TrackGrid.ViewModel
{
    public class SortSettings
    {
        public SortDirection Direction { get; private set; }

        public int? Rank { get; private set;  }


        public SortSettings() 
        {
            Reset();
        }

        /// <summary>
        /// Sets the next sort direction, while keeping account of the column sort rank.
        /// When a non-null <paramref name="sortRank"/> is supplied, the rank will be set accordingly.
        /// When a non-null <paramref name="sortRank"/> is not given, and <see cref="SortSettings.Rank"/> is null, the column is presumed to be the primary one.
        /// </summary>
        public void SetNextSortDirection(int? sortRank = null)
        {
            if (sortRank is not null)
                Rank = sortRank;

            else if (Rank is null)
                Rank = 0;

            Direction = GetNextSortDirection();
        }

        public void Reset()
        {
            Direction = SortDirection.Unsorted;
            Rank = null;
        }

        private SortDirection GetNextSortDirection()
        {
            return Direction switch
            {
                SortDirection.Unsorted => SortDirection.Ascending,
                SortDirection.Ascending => SortDirection.Descending,
                SortDirection.Descending => SortDirection.Ascending,
                _ => throw new ArgumentException($"No such {nameof(SortDirection)}.")
            };
        }
    }
}
