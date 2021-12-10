using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TailSpin.SpaceGame.Web.Models;

namespace TailSpin.SpaceGame.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILeaderboardServiceClient _leaderboardServiceClient;

        public HomeController(ILeaderboardServiceClient leaderboardServiceClient)
        {
            this._leaderboardServiceClient = leaderboardServiceClient;
        }

        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string mode = "",
            string region = ""
            )
        {
            // Create the view model with initial values we already know.
            var vm = new LeaderboardViewModel();
            vm.GameModes = new List<string>()
                {
                    "Solo",
                    "Duo",
                    "Trio"
                };

            vm.GameRegions = new List<string>()
                {
                    "Milky Way",
                    "Andromeda",
                    "Pinwheel",
                    "NGC 1300",
                    "Messier 82",
                };

            vm.PageSize = pageSize;

            try
            {
                // Call the leaderboard service with the provided parameters.
                LeaderboardResponse leaderboardResponse = await this._leaderboardServiceClient.GetLeaderboard(page, pageSize, mode, region);

                vm.Page = leaderboardResponse.Page;
                vm.PageSize = leaderboardResponse.PageSize;
                vm.Scores = leaderboardResponse.Scores;
                vm.SelectedMode = leaderboardResponse.SelectedMode;
                vm.SelectedRegion = leaderboardResponse.SelectedRegion;
                vm.TotalResults = leaderboardResponse.TotalResults;

                // Set previous and next hyperlinks.
                if (page > 1)
                {
                    vm.PrevLink = $"/?page={page - 1}&pageSize={pageSize}&mode={mode}&region={region}#leaderboard";
                }
                if (vm.TotalResults > page * pageSize)
                {
                    vm.NextLink = $"/?page={page + 1}&pageSize={pageSize}&mode={mode}&region={region}#leaderboard";
                }
            }
            catch(Exception e)
            {
                vm.ErrorMessage = $"Unable to retrieve leaderboard: {e}";
                Trace.TraceError(vm.ErrorMessage);
            }

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
