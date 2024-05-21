using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class ClientTools
    {
        public static string GetClient(string? line)
        {
            if (line.Split(' ').Length < 2 || line.Split(' ')[1] == "")
                return "none";
            string type = line.Split(' ')[1];
            return type;
        }

        public static string GetCommunicator(string? line)
        {
            if (String.IsNullOrEmpty(line))
                return "none";
            string type = line.Split(' ')[0];
            return type;
        }

        public static string GetFilePath(string filePath, string fileName)
        {
            int counter = 0;
            string fullPath = filePath + Path.GetFileName(fileName);


            while (File.Exists(fullPath))
            {
                counter++;
                fullPath = $"{filePath}{Path.GetFileNameWithoutExtension(fileName)}({counter}){Path.GetExtension(fullPath)}";
            }
            return fullPath;
        }

        public static bool PingClientParamsHandler(string[] paramsPing)
        {
            int number1, number2, number3;
            if (int.TryParse(paramsPing[2], out number1) && int.TryParse(paramsPing[3], out number2) && int.TryParse(paramsPing[4], out number3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
