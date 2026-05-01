using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MauiUiApp.repository
{
    // <summary>
    // Вызываем бэкенд для получение информации из PDF
    // </summary>
    // <param name="path">Ссылка на файл</param>
    // <param name="number_pipe">Номер пайпа для подключения</param>
    // <returns>Строка с парсиными данными</returns>
    public class PipeFileRepository
    {
        public static async Task<string> processing(string path, int number_pipe)
        {
            MyPipe pipe = new MyPipe();
            await pipe.InstancPipe("Pipe_number_" + number_pipe.ToString());

            string publishDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string appPath = Path.Combine(publishDirectory, "MauiApp", "MauiApp.exe");

            string arg1 = path;
            string arg2 = "Pipe_number_" + number_pipe.ToString();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = appPath,
                Arguments = $"{arg1} {arg2}",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            Process process = Process.Start(startInfo);

            await pipe.server.WaitForConnectionAsync();
            await pipe.ReadMessage();

            return pipe.receivedMessage;
        }
    }
}
