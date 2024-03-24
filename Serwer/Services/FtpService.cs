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
        public string AnswerCommand(string command)
        {
            string filePath = @"C:\ftpS\tmp";
            if (File.Exists(filePath))
            {
                using (FileStream fs = File.Open(filePath, FileMode.Append, FileAccess.Write))
                {
                    //question += File.ReadAllText(filePath);
                    //question += "\n";
                    Console.WriteLine("Reading");

                }

                //Console.WriteLine(question);
            }
            else
            {
                Console.WriteLine("Could not find location : " + filePath);
            }

            //int n = CommonTools.GetAnswerLength(command);
            return command + "\n";
        }
    }
}
