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
            //string loc = @"C:\ftpS\";
            string[] lines = command.Split(" ");
            string fileName = lines[lines.Length - 2];
            string filePath = @$"{ServerTools.FileExistCounter(loc, loc + fileName)}";
            return filePath;
        }
    }
}
