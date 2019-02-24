using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker
{
    public static class TeamList
    {
        private static List<TeamInfo> teams;

        public static List<TeamInfo> Teams
        {
            get { return teams; }
            set { teams = value; }
        }

        public static void LoadTeamList()
        {
            Teams = DB.Helper.LoadList<TeamInfo>("SELECT * FROM TeamTbl");

        }

        
        //****************************
        // Helper Methods
        //****************************
        public static string GetNameFromKey(int key)
        {
            return Teams.FirstOrDefault(x => x.TeamKey == key)?.TeamName.ToString();
        }

        public static string GetFullNameFromKey(int key)
        {
            return Teams.First(x => x.TeamKey == key)?.FullName.ToString();
        }

        public static string GetCityFromKey(int key)
        {
            return Teams.First(x => x.TeamKey == key)?.City.ToString();
        }

        public static int GetKeyFromWebsiteName(string name)
        {
            var result = Teams.First(x => x.WebsiteTeamName == name);
            return result.TeamKey;
        }

    }




    public class TeamInfo
    {
        // DB Properties
        public int TeamKey { get; set; }
        public string City { get; set; }
        public string TeamName { get; set; }
        public string League { get; set; }
        public string Symbol { get; set; }
        public string WebsiteTeamName { get; set; }


        // Properties
        public string FullName { get { return City + " " + TeamName; }  }
    }

}
