using Newtonsoft.Json;
using Questao2;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int goalsAsTeam1 = GetGoalsFromApi(team, year, "team1").Result;
        int goalsAsTeam2 = GetGoalsFromApi(team, year, "team2").Result;

        return goalsAsTeam1 + goalsAsTeam2;
    }

    static async Task<int> GetGoalsFromApi(string team, int year, string role)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;

        using (HttpClient client = new())
        {
            do
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{role}={Uri.EscapeDataString(team)}&page={currentPage}";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var footballMatchesResponse = JsonConvert.DeserializeObject<FootballMatchesResponse>(responseBody)
                    ?? throw new NullReferenceException();

                if (currentPage == 1)
                    totalPages = footballMatchesResponse.TotalPages;

                totalGoals += footballMatchesResponse.Data.Where(x => x.Team1 == team).Sum(t => int.Parse(t.Team1goals));
                totalGoals += footballMatchesResponse.Data.Where(x => x.Team2 == team).Sum(t => int.Parse(t.Team2goals));

                currentPage++;
            } while (currentPage <= totalPages);
        }

        return totalGoals;
    }
}