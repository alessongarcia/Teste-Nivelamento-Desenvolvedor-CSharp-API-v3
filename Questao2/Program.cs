using Newtonsoft.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int GetTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        totalGoals += GetGoalsFromApi(team, year, "team1");
        totalGoals += GetGoalsFromApi(team, year, "team2");
        return totalGoals;
    }
    private static int GetGoalsFromApi(string team, int year, string teamPosition)
    {
        int goals = 0;
        int page = 1;
        int totalPages = 1;

        using HttpClient client = new HttpClient();

        while (page <= totalPages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamPosition}={Uri.EscapeDataString(team)}&page={page}";
            var response = client.GetAsync(url);
            var json = response.Result.Content.ReadAsStringAsync().Result;

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            totalPages = root.GetProperty("total_pages").GetInt32();
            JsonElement data = root.GetProperty("data");

            foreach (JsonElement match in data.EnumerateArray())
            {
                string goalsString = match.GetProperty($"{teamPosition}goals").ToString();
                if (int.TryParse(goalsString, out int matchGoals))
                {
                    goals += matchGoals;
                }
            }

            page++;
        }

        return goals;
    }
}