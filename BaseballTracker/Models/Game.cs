using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker.Models
{
    public class Game
    {
        public int GameKey { get; set; }
        public Team Home { get; set; }
        public Team Away { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
        public int HomeTeamKey { get; set; }
        public int AwayTeamKey { get; set; }
        public string Date { get; set; }
        public int WinnerKey { get; set; }

        public Game()
        {
            GameKey = DB.Helper.GetNextGameKey();

            Home = new Team();
            Away = new Team();
        }


    }
}
