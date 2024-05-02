using Commons;
using Serwer.Interfaces;
using Serwer.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    internal class FtpService : IServiceModule
    {
        private string filePath = @"C:\ftpS\";
        public string AnswerCommand(string command)
        {
            string commandType = ServerTools.DetectCommandType(command);
            string answer;
            switch(commandType)
            {
                case "put":
                    answer = PutService(command);
                    break;
                case "get":
                    answer = GetService(command);
                    break;
                case "list":
                    answer = ListService(command);
                    break;
                default:
                    return "Wrong command option! \n";
            }
            return answer;
        }

        private string GetService(string command)
        {
            string fileName = command.Split(" ")[2];
            if (File.Exists(this.filePath+fileName))
            {
                byte[] bytes = File.ReadAllBytes(filePath+fileName);
                string file = Convert.ToBase64String(bytes);
                return $"Sending file {file} \n";
            }
            else
            {
                return "Could not find file : " + fileName + "\n";
            }
        }

        private string ListService(string command)
        {
            string[] dirFileList = Directory.GetFiles(@"C:\ftpS\");
            string fileList = "";
            foreach(string file in dirFileList)
            {
                fileList += Path.GetFileName(file).ToString() + "\t";
            }
            return fileList + "\n";
        }

        private string PutService(string command)
        {
            string filePath = ServerTools.GetFilePath(command, this.filePath);
            string bytes = CommonTools.ScrapBytes(command);
            try
            {
                File.WriteAllBytes(filePath, Convert.FromBase64String(bytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "File uploaded as "+ Path.GetFileName(filePath) + "\n";
        }
    }
}
