using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace TailSpin.SpaceGame.Web
{
    public class LeaderboardFunctionClient : ILeaderboardServiceClient
    {
        private string _functionUrl;

        public LeaderboardFunctionClient(string functionUrl)
        {
            this._functionUrl = functionUrl;
        }

        async public Task<LeaderboardResponse> GetLeaderboard(int page, int pageSize, string mode, string region)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetFromJsonAsync<LeaderboardResponse>($"{this._functionUrl}?page={page}&pageSize={pageSize}&mode={mode}&region={region}");
            }
        }
    }
}
