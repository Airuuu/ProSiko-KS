using Commons;
using Klient.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Clients
{
    internal class FtpClient : QAClient
    {
        private string filePath = @"C:\ftpC\";

        public FtpClient(ClientCommunicator clientCommunicator) : base(clientCommunicator)
        {
        }

        public double FtpPut(string filePath)
        {
            string question = $"ftp put ";

            if (File.Exists(filePath))
            {
                DateTime startTime = DateTime.Now;
                byte[] bytes = File.ReadAllBytes(filePath);
                string file = Convert.ToBase64String(bytes);
                question += $"{file.ToString()} {CommonTools.ScrapFileName(filePath)} \n";
                string answer = clientCommunicator.QA(question);
                DateTime endTime = DateTime.Now;
                double duration = (endTime - startTime).TotalSeconds;
                return duration;
            }
            else
            {
                Console.WriteLine("Could not find location : " + filePath);
            }
            return 0;
        }

        public double FtpList()
        {
            string question = $"ftp list \n";
            DateTime startTime = DateTime.Now;
            string answer = clientCommunicator.QA(question);
            Console.WriteLine($"Files on the server : \n {answer}");
            DateTime endTime = DateTime.Now;
            double duration = (endTime - startTime).TotalSeconds;
            return duration;
        }

        public double FtpGet(string fileName)
        {
            DateTime startTime = DateTime.Now;
            string question = $"ftp get {fileName} \n";
            string answer = clientCommunicator.QA(question);
            string bytes = CommonTools.ScrapBytes(answer);
            string filePath = ClientTools.GetFilePath(this.filePath, fileName);
            File.WriteAllBytes(filePath, Convert.FromBase64String(bytes));
            DateTime endTime = DateTime.Now;
            double duration = (endTime - startTime).TotalSeconds;
            return duration;
        }
    }
}
