using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    internal class FileCommunicator : ClientCommunicator
    {
        string requestPath;
        string responsePath;
        FileSystemWatcher watcher;

        string UID;
        string requestFile;
        string infoFile;

        public FileCommunicator()
        {
            Thread.Sleep(1000);
            requestPath = @"C:\FC\requests";
            responsePath = @"C:\FC\responses";

            UID = Guid.NewGuid().ToString();
            watcher = new FileSystemWatcher();
        }

        public override string QA(string question)
        {
            string response = string.Empty;
            object locker = new object();
            bool responseFlag = false;


            //Create files containing the request file and the monitoring file
            requestFile = Path.Combine(requestPath, $"{UID}.txt");
            infoFile = Path.Combine(requestPath, $"{UID}_info.txt");

            //System watcher config
            watcher.Path = responsePath;
            watcher.Filter = $"{UID}_info.txt";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            //Start monitoring
            watcher.EnableRaisingEvents = true;

            //Ensure that the code in it is executed by only one thread at a time
            lock (locker)
            {
                File.WriteAllText(requestFile, question);
                File.WriteAllText(infoFile, response);

                //Subscribe the event
                watcher.Created += (source, fileEvent) =>
                {
                    //Synchronize
                    lock (locker)
                    {
                        //If the answer is not processed yet
                        if (!responseFlag)
                        {
                            responseFlag = true;

                            string path = fileEvent.FullPath.Split("_").First() + ".txt";
                            response = File.ReadAllText(path);

                            Monitor.Pulse(locker);

                            File.Delete(fileEvent.FullPath);
                            File.Delete(path);
                        }

                    }
                };

                //Wait for answer
                Monitor.Wait(locker);
            }

            watcher.EnableRaisingEvents = false;

            return response;
        }

        public override void Dispose()
        {
            watcher.Dispose();
        }
    }
}
