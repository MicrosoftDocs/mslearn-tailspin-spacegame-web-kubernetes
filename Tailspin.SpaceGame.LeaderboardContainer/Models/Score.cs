using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TailSpin.SpaceGame.LeaderboardContainer
{
    public class Score : Model
    {
        // The ID of the player profile associated with this score.
        [JsonProperty(PropertyName = "profileId")]
        [JsonPropertyName("profileId")]
        public string ProfileId { get; set; }

        // The score value.
        [JsonProperty(PropertyName = "score")]
        [JsonPropertyName("score")]
        public int HighScore { get; set; }

        // The game mode the score is associated with.
        [JsonProperty(PropertyName = "gameMode")]
        [JsonPropertyName("gameMode")]
        public string GameMode { get; set; }

        // The game region (map) the score is associated with.
        [JsonProperty(PropertyName = "gameRegion")]
        [JsonPropertyName("gameRegion")]
        public string GameRegion { get; set; }
    }
}