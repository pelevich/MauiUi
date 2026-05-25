using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MauiUiApp.Application
{
    public class ClientApi
    {
        private readonly HttpClient _httpClient;

        // Создаем клиента, подключаемся к нужному url
        public ClientApi(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // <summary>
        // Отправка POST запроса
        // </summary>
        // <param name="endpoint">куда отправлять файл</param>
        // <param name="fileDto">файл</param>
        // <return>json ответ от api</param>
        public async Task<string> PostAsync(string endpoint, FileUploadDto fileDto)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(fileDto.FileData), fileDto.fileFieldName, fileDto.FileName);

            var response = await _httpClient.PostAsync(endpoint, form); ;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
