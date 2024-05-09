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

        public void FtpPut(string filePath)
        {
            string question = $"ftp put ";

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                string file = Convert.ToBase64String(bytes);
                question += $"{file.ToString()} {CommonTools.ScrapFileName(filePath)} \n";
                string answer = clientCommunicator.QA(question);
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine("Could not find location : " + filePath);
            }
        }

        public void FtpList()
        {
            string question = $"ftp list \n";
            string answer = clientCommunicator.QA(question);
            Console.WriteLine($"Files on the server : \n {answer}");
        }

        public void FtpGet(string fileName)
        {
            string question = $"ftp get {fileName} \n";
            string answer = clientCommunicator.QA(question);
            string bytes = CommonTools.ScrapBytes(answer);
            string filePath = ClientTools.GetFilePath(this.filePath, fileName);
            File.WriteAllBytes(filePath, Convert.FromBase64String(bytes));
        }
    }
}
