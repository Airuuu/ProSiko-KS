using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Commons
{
    public class ServerTools
    {

        public static string DetectCommandType(string command)
        {
            return command.Split(' ')[1];
        }
        public static string FileExistCounter(string loc, string fullPath)
        {
            int counter = 0;
            string fileName = Path.GetFileNameWithoutExtension(fullPath);


            while (File.Exists(fullPath))
            {
                counter++;
                fullPath = $"{loc}{fileName}({counter}){Path.GetExtension(fullPath)}";
            }
            return fullPath;
        }

        public static string GetConfigName(string command)
        {
            return command.Split(" ")[2].ToLower();
        }

        public static string GetFilePath(string command, string loc)
        {
            string[] lines = command.Split(" ");
            string fileName = lines[lines.Length - 2];
            string filePath = @$"{ServerTools.FileExistCounter(loc, loc + fileName)}";
            return filePath;
        }

        public static bool GetSpecifiedState(string serviceName, string allStates)
        {
            string[] parts = allStates.Split(" ");
            for (int i = 0; i < parts.Length; i++)
            {
                string[] itemAndState = parts[i].Split("\t");
                if (itemAndState[0] == serviceName && itemAndState[1].ToLower() == "online")
                    return true;
                if (itemAndState[0] == serviceName && itemAndState[1].ToLower() == "offline")
                    return false;
            }
            return false;
        }

        public static bool GetSpecifiedServiceState(string serviceName, List<string> services)
        {
            if (services.Contains(serviceName))
                return true;
            return false;
        }

        public static void CheckDirectories()
        {
            Directory.CreateDirectory(@"C:\FC\requests");
            Directory.CreateDirectory(@"C:\FC\responses");
            Directory.CreateDirectory(@"C:\ftpS");
            Directory.CreateDirectory(@"C:\ftpC");
        }
    }
}
