using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace MauiUiApp.repository
{
    internal class MyPipe
    {
        private readonly CancellationTokenSource _cts = new();
        public NamedPipeServerStream server;
        public string receivedMessage;
        byte[] buffer = new byte[256];

        // <summary>
        // Инициализируем пайп
        // </summary>
        // <param name="name">Имя пайпа</param>
        public async Task InstancPipe(string name)
        {
            server = new NamedPipeServerStream(
            name,
            PipeDirection.InOut,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);

        }

        // <summary>
        // Читаем сообщение от бэкенда
        // </summary>
        public async Task ReadMessage()
        {
            int bytesRead = await server.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
            receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        // <summary>
        // Отправляем сообщение
        // </summary>
        // <param name="response">Отправляемое сообщение</param>
        public async Task WriteMessage(string response)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await server.WriteAsync(responseBytes, 0, responseBytes.Length, _cts.Token);
        }
    }
}