using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker
{
    public class Logger
    {
        static string logPath = "ProcessLog.txt";
        static string consolePath = "ConsoleLog.txt";

        public static void log(string str)
        {
            DateTime now = DateTime.Now;
            string date = now.ToShortDateString();
            string time = now.ToShortTimeString();
            string dateTime = date + " " + time;

            using (StreamWriter streamWriter = new StreamWriter(logPath, true))
            {
                streamWriter.WriteLine(dateTime + ": " + str);
                streamWriter.Close();
            }

        }

        public static void consoleLog(string str)
        {

            using (StreamWriter streamWriter = new StreamWriter(consolePath, true))
            {
                streamWriter.WriteLine(str);
                streamWriter.Close();
            }

        }

        public static void CreateFile(string pathName, string str)
        {
            pathName = @"..\..\..\" + pathName + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(pathName, true))
            {
                streamWriter.WriteLine(str);
                streamWriter.Close();
            }
        }

        public static string ReadFile(string pathName)
        {
            pathName = @"..\..\..\" + pathName + ".txt";
            return File.ReadAllText(pathName);
        }

    }
}
