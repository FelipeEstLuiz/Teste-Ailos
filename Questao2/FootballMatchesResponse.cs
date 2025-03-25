using Newtonsoft.Json;

namespace Questao2
{
    internal class FootballMatchesResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<FootballMatchesResponseData> Data { get; set; } = new List<FootballMatchesResponseData>();
    }

    internal class FootballMatchesResponseData
    {
        [JsonProperty("competition")]
        public string Competition { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }

        [JsonProperty("team1")]
        public string Team1 { get; set; }

        [JsonProperty("team2")]
        public string Team2 { get; set; }

        [JsonProperty("team1goals")]
        public string Team1goals { get; set; }

        [JsonProperty("team2goals")]
        public string Team2goals { get; set; }
    }
}
