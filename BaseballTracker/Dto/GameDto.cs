using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker.Dto
{
    class GameDto
    {
        public int GameKey { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
        public int HomeTeamKey { get; set; }
        public int AwayTeamKey { get; set; }
        public string Date { get; set; }
        public int WinnerKey { get; set; }


    }
}
