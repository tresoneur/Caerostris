using Caerostris.Shared.Data.TrackGrid.ViewModel;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Caerostris.Shared.Data.TrackGrid.Interfaces
{
    public interface IColumn
    {
        public string Title { get; }

        public string Class { get; }

        public int MinWidthPx { get; }

        public bool IsFixedWidth { get; set; }

        public bool IsVisible { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }


        public SortSettings SortSettings { get; set; }


        public bool IsFilterable { get; }
        
        public string? FilterQuery { get; set; }


        public RenderFragment<Track> FieldTemplate { get; set; }


        /// <summary>
        /// Hierarchical sorting. Invoked by the Grid in the right order and only on sorted columns.
        /// </summary>
        /// <remarks>The method should use ThenBy() or equivalent.</remarks>
        public IOrderedEnumerable<Track> Sort(IOrderedEnumerable<Track> tracks);

        /// <summary>
        /// Filtering. Invoked by the Grid only on columns on which a valid FilterQuery is set.
        /// </summary>
        public IEnumerable<Track> Filter(IEnumerable<Track> tracks, string filterQuery);
    }
}
