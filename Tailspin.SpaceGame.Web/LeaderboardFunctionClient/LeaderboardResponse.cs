using System.Collections.Generic;
using TailSpin.SpaceGame.Web.Models;

namespace TailSpin.SpaceGame.Web
{
    public class LeaderboardResponse
    {
        // The game mode selected in the view.
        public string SelectedMode { get; set; }
        // The game region (map) selected in the view.
        public string SelectedRegion { get; set; }
        // The current page to be shown in the view.
        public int Page { get; set; }
        // The number of items to show per page in the view.
        public int PageSize { get; set; }

        // The scores to display in the view.
        public IEnumerable<ScoreProfile> Scores { get; set; }
        // The total number of results for the selected game mode and region in the view.
        public int TotalResults { get; set; }
    }
}