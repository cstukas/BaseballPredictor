using BaseballTracker.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker.Web
{
    public static class Parser
    {

        public static List<Game> ParseMLBScores(string scoresResult, string date)
        {
            List<Game> games = new List<Game>();
            scoresResult = scoresResult.Replace("><", ">|<");
            HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(scoresResult);

            string mainGameClass = "//div[@class='EventCard__eventCardContainer--3hTGN']";
            var textNodes = htmlDoc.DocumentNode.SelectNodes(mainGameClass);
            if (textNodes != null)
            {
                foreach (HtmlNode t in textNodes)
                {
                    Game game = CreateGameScoreObject(t.InnerText);

                    HtmlNode linkNode = t.SelectSingleNode("span/a");
                    if (linkNode != null)
                    {
                        string link = linkNode.Attributes["href"].Value;
                        game.Link = link;
                   //     game = parseGamePageForPitchers(game);
                    }

                    game.Date = date;
                    games.Add(game);
                }
            }
            Logger.consoleLog("");
            return games;
        }

        //returns game object from webresponse
        public static Game CreateGameScoreObject(string stringToParse)
        {
            string newStrToParse = removeExtraBars(stringToParse);
            string[] parseArr = newStrToParse.Split('|');
            int arrCount = parseArr.Length;

            Game game = new Game();

            try
            {

                if (newStrToParse.ToLower().Contains("postponed") || arrCount <= 5)
                {
                    game.Away.Name = parseArr[1];
                    game.AwayTeamKey = TeamList.GetKeyFromWebsiteName(parseArr[1]);

                    game.Status = parseArr[2];
                    game.Home.Name = parseArr[3];
                    game.HomeTeamKey = TeamList.GetKeyFromWebsiteName(parseArr[3]);
                }
                else
                {
                    game.Away.Name = parseArr[1];
                    game.AwayTeamKey = TeamList.GetKeyFromWebsiteName(parseArr[1]);


                    game.Away.Score = parseArr[2];
                    game.Status = parseArr[3];

                    if (arrCount > 5)
                    {
                        game.Home.Name = parseArr[4];
                        game.HomeTeamKey = TeamList.GetKeyFromWebsiteName(parseArr[4]);
                    }
                       

                    if (arrCount > 6)
                        game.Home.Score = parseArr[5];
                }
                
            }
            catch (Exception e)
            {
                throw;
            }

            if (game.Status.ToLower().Contains("final"))
            {
                int homeScore = Convert.ToInt32(game.Home.Score);
                int awayScore = Convert.ToInt32(game.Away.Score);

                if (homeScore > awayScore)
                {
                    game.WinnerKey = game.HomeTeamKey;
                }
                if (homeScore < awayScore)
                {
                    game.WinnerKey = game.AwayTeamKey;
                }
            }

            return game;
        }
        

        private static string removeExtraBars(string str)
        {
            string newStr = str;
            for (int i = 0; i < 10; i++)
            {
                newStr = newStr.Replace("||", "|");
            }
            return newStr;
        }
    }
}
