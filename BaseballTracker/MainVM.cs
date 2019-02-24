using BaseballTracker.Dto;
using BaseballTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker
{
    class MainVM : BaseClasses.ViewModelBase
    {
        // Games to show on UI
        private List<GameDto> displayGames;

        public List<GameDto> DisplayGames
        {
            get { return displayGames; }
            set { displayGames = value; OnPropertyChanged("DisplayGames"); }
        }


        public MainVM()
        {
            // Load Info
            TeamList.LoadTeamList();
            var test = TeamList.Teams;

            PullAndInsertGameInfo(0, 0);

            SetDisplayGames();
        }


        public void PullAndInsertGameInfo(int lookbackDays, int lookForwardDays)
        {
            DateTime now = DateTime.Now;
        //    now = new DateTime(2019, 2, 25, 5, 5, 5);//TESTING
            DateTime startTime = now.AddDays(lookbackDays);
            DateTime endTime = now.AddDays(lookForwardDays);
            while (startTime <= endTime)
            {
                string date = startTime.ToString("yyyy-MM-dd");
                string fileName = "scores2";
                string scoresResult = ScrapeGameInfo(fileName, date);

                List<Game> games = Web.Parser.ParseMLBScores(scoresResult, date);
                DB.Helper.ClearTable("GameTbl", "WHERE Date = '" + date + "'");
                InsertGamesList(games);

                startTime = startTime.AddDays(1);
            }
        }

        public string ScrapeGameInfo(string fileName, string date)
        {

            string link = Links.GameURL + date;
            string scoresResult = Web.Scraper.Scrape(link);
            Logger.CreateFile(fileName, scoresResult);
            return scoresResult;
        }


        void InsertGamesList(List<Game> games)
        {
            for (int i = 0; i < games.Count; i++)
            {
                Game game = games[i];
                GameDto dto = Utils.Map<Game, GameDto>(game);
                DB.Helper.InsertObject<GameDto>(dto, "GameTbl");
            }


        }

        void SetDisplayGames()
        {
            DisplayGames = DB.Helper.LoadList<GameDto>("SELECT * FROM GameTbl ORDER BY GameKey");


        }

    }
}
