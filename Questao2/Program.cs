using Newtonsoft.Json;
using System.Text.RegularExpressions;


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


    public class Match
    {

        public string Competition { get; set; }
        public int Year { get; set; }
        public string Round { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
    }

    public class MatchData
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<Match> Data { get; set; }
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        Program program = new Program();
        program.IsTimeValido(team);

        var totaldegolstime1 = 0;

        for (int i = 1; i < 4; i++)
        {
            string dadostime1 = PegaDados(team, year, "&team1=", "&page=", i).GetAwaiter().GetResult();




            MatchData dadosDaPartida = JsonConvert.DeserializeObject<MatchData>(dadostime1);
            
            foreach (var partida in dadosDaPartida.Data)
            {
                totaldegolstime1 += partida.Team1Goals;
            }
        }
        var totaldegolstime2 = 0;

        for (int i = 1; i < 4; i++)
        {
            string dadostime2 = PegaDados(team, year, "&team2=", "&page=", i).GetAwaiter().GetResult();

            MatchData dadosDaPartida2 = JsonConvert.DeserializeObject<MatchData>(dadostime2);
           
            foreach (var partida in dadosDaPartida2.Data)
            {
                totaldegolstime2 += partida.Team2Goals;
            }
        }

        var GolsTotais = totaldegolstime1 + totaldegolstime2;

        return GolsTotais;
    }

    private bool IsTimeValido(string time)
    {
        if (time == "Paris Saint-Germain" || time == "Chelsea")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    static async Task<string> PegaDados(string nometime, int ano, string time,string pagina,int indice)
    {
        if (!int.TryParse(ano.ToString(), out int validAno) || string.IsNullOrWhiteSpace(nometime))
        {
            throw new ArgumentException("Input Invalido");
        }

        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://jsonmock.hackerrank.com/api/football_matches?year=" + ano + time + nometime + pagina + indice);
        var textContent = await response.Content.ReadAsStringAsync();

        httpClient.Dispose();

        return textContent;
    }

}