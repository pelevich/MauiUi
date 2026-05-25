using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace MauiUiApp.Application 
{
    public class StartWebApi : IDisposable
    {
        private Process _webApiProcess;
        private bool _disposed;
        public string _addres = "http://localhost:5000";

        //Запускает api, когда открывается ui
        public async Task StartAsync()
        {
            string publishDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string releaseDirectory = Directory.GetParent(Directory.GetParent(publishDirectory).FullName).FullName;
            string appPath = Path.Combine(releaseDirectory, "WebApi", "WebApp.exe");
            Debug.WriteLine(appPath);
            _webApiProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            _webApiProcess.Start();
        }

        //Заверщение api
        public void Stop()
        {
            if (_webApiProcess != null && !_webApiProcess.HasExited)
            {
                _webApiProcess.Kill();
                _webApiProcess.WaitForExit(5000);
                _webApiProcess.Dispose();
                _webApiProcess = null;
            }
        }
        //Явное освобождения ресурсов
        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}
