using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TailSpin.SpaceGame.LeaderboardContainer;

namespace Tailspin.SpaceGame.LeaderboardContainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        // High score repository.
        private readonly IDocumentDBRepository<Score> _scoreRepository;

        // User profile repository.
        private readonly IDocumentDBRepository<Profile> _profileRespository;

        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(ILogger<LeaderboardController> logger,
            IDocumentDBRepository<Score> scoreRepository,
            IDocumentDBRepository<Profile> profileRespository)
        {
            this._logger = logger;
            this._scoreRepository = scoreRepository;
            this._profileRespository = profileRespository;
        }

        [HttpGet]
        async public Task<LeaderboardResponse> Get(int page, int pageSize, string mode, string region)
        {
            // Create the baseline response.
            var leaderboardResponse = new LeaderboardResponse()
            {
                Page = page,
                PageSize = pageSize,
                SelectedMode = mode,
                SelectedRegion = region
            };

            // Form the query predicate.
            // This expression selects all scores that match the provided game 
            // mode and region (map).
            // Select the score if the game mode or region is empty.
            Expression<Func<Score, bool>> queryPredicate = score =>
                (string.IsNullOrEmpty(mode) || score.GameMode == mode) &&
                (string.IsNullOrEmpty(region) || score.GameRegion == region);

            // Fetch the total number of results in the background.
            var countItemsTask = this._scoreRepository.CountItemsAsync(queryPredicate);

            // Fetch the scores that match the current filter.
            IEnumerable<Score> scores = await this._scoreRepository.GetItemsAsync(
                queryPredicate, // the predicate defined above
                score => score.HighScore, // sort descending by high score
                page - 1, // subtract 1 to make the query 0-based
                pageSize
              );

            // Wait for the total count.
            leaderboardResponse.TotalResults = await countItemsTask;

            // Fetch the user profile for each score.
            // This creates a list that's parallel with the scores collection.
            var profiles = new List<Task<Profile>>();
            foreach (var score in scores)
            {
                profiles.Add(this._profileRespository.GetItemAsync(score.ProfileId));
            }
            Task<Profile>.WaitAll(profiles.ToArray());

            // Combine each score with its profile.
            leaderboardResponse.Scores = scores.Zip(profiles, (score, profile) => new ScoreProfile { Score = score, Profile = profile.Result });

            return leaderboardResponse;
        }
    }
}
