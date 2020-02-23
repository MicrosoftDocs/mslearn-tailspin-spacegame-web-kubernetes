using Newtonsoft.Json;

namespace TailSpin.SpaceGame.LeaderboardContainer
{
    /// <summary>
    /// Base class for data models.
    /// </summary>
    public abstract class Model
    {
        // The value that uniquely identifies this object.
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}