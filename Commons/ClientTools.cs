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

        public static void PrintManual()
        {
            Console.WriteLine("Available media:\n" +
                "file\ttcp\tudp\tcom\tgrpc\n\n" +
                "Avaiable services:\n" +
                "ping [n] [intputLen] [outputLen]- check server response\n" +
                "Args: \n" +
                "\tn-times\n" +
                "\tinputLen - input length\n" +
                "\toutputLen - output length\n\n" +
                "ftp [action] [file] - manage the files\n" +
                "Args: \n" +
                "\taction - put | get | list\n" +
                "\tfile - file you want to get/upload (not required for list)\n\n" +
                "chat [action] [destinationUser] [message]\n" +
                "\taction - msg | get\n" +
                "\tif action is msg:\n" +
                "\t\tdestinationUser - user you want to send the message\n" +
                "\t\tmessage - your message\n\n" +
                "conf [action]\n" +
                "Args: \n" +
                "action - get-states | start-service | stop-service | start-medium | stop-medium\n\n" +
                "test - testing times for all communicators\n\n" +
                "Example inputs:\n" +
                "tcp ping 10 1024 2048\n" +
                "com ftp list\n" +
                "udp ftp get ugabuga.txt\n" +
                "file message get\n" +
                "grpc conf get-states\n" +
                "tcp conf start-module udp\n");
        }
    }
}
