using Serwer.Communicators;
using Serwer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Listeners
{
    internal class FileListener : IListener
    {
        bool shouldTerminate;
        CommunicatorD onConnect;
        Thread _thread;
        FileSystemWatcher watcher;
        string requestPath;
        string responsePath;

        public FileListener()
        {
            requestPath = @"C:\FC\requests";
            responsePath = @"C:\FC\responses";
        }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            shouldTerminate = false;
            watcher = new FileSystemWatcher();
            _thread = new Thread(Listen);
            _thread.Start();
        }

        private void Listen()
        {
            watcher.Path = requestPath;
            watcher.Filter = "*_info.txt";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            watcher.Created += OnRequestReceived;

            watcher.EnableRaisingEvents = true;

            while (!shouldTerminate) { }
        }

        private void OnRequestReceived(object sender, FileSystemEventArgs fileEvent)
        {
            FileCommunicator fileCommunicator = new FileCommunicator(fileEvent.FullPath);
            onConnect(fileCommunicator);
        }

        public void Stop()
        {
            shouldTerminate = true;
            watcher.Dispose();
        }
    }
}
