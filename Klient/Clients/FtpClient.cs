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
        public FtpClient(ClientCommunicator clientCommunicator) : base(clientCommunicator)
        {
        }

        public double Test(string filePath)
        {
            string question = $"ftp ";

            //question += CommonTools.Trush(outputLen - question.Length - 1);
            //question += '\n';

            if (File.Exists(filePath))
            {
                DateTime startTime = DateTime.Now;
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    question += File.ReadAllText(filePath);
                    question += "\n";

                    string answer = clientCommunicator.QA(question);
                    DateTime endTime = DateTime.Now;
                    double duration = (endTime - startTime).TotalSeconds;
                    return duration;
                }

                //Console.WriteLine(question);
            }
            else
            {
                Console.WriteLine("Could not find location : " + filePath);
            }
            return 0;



            //for (int i = 0; i < amount; i++)
            //{
            //    //Console.WriteLine($"S: {question}");
            //    string answer = clientCommunicator.QA(question);
            //    //Console.WriteLine($"R: {answer}");
            //}

        }
    }
}
