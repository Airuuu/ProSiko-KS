using Commons;
using Serwer.Interfaces;
using Serwer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    internal class FileCommunicator : ICommunicator
    {
        private string fullPath;
        private CommandD onCommand;
        private CommunicatorD onDisconnect;
        private Thread thread;

        public FileCommunicator(string path)
        {
            fullPath = path;
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            this.onCommand = onCommand;
            this.onDisconnect = onDisconnect;
            thread = new Thread(Communicate);
            thread.Start();
        }

        private void Communicate()
        {
            try
            {
                string path = fullPath.Split("_").First() + ".txt";
                string line = File.ReadAllText(path);

                string answer = ProcessCommand(line);

                File.Delete(fullPath);
                File.Delete(path);

                string responseFile = Path.Combine(@"C:\FC\responses", Path.GetFileName(path));
                string responseInfo = Path.Combine(@"C:\FC\responses", Path.GetFileName(fullPath));

                File.WriteAllText(responseFile, answer);
                File.WriteAllText(responseInfo, "");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            onDisconnect(this);
        }

        private string ProcessCommand(string line)
        {
            ConfigService config = new ConfigService();
            string configAnswer = onCommand("conf get-states");
            string answer = "";
            if (line.Split(" ")[0] != "conf" && !ServerTools.GetSpecifiedState(line.Split(" ")[0], configAnswer))
            {
                answer += "Error : Service is OFFLINE! \n";
            }
            else
            {
                answer += onCommand(line);
            }
            return answer;
        }

        public void Stop()
        {

        }
    }
}
