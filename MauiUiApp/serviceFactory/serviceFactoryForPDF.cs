using MauiUiApp.service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace MauiUiApp.serviceFactory
{
    public class serviceFactoryForPDF : IServiceFactory
    {

        private int count_pipe = 1;

        // <summary>
        // Вызывает нужный метод из слоя repository и так же достает из полного пути имя файла
        // </summary>
        // <param name="path">путь до обрабатываемого файла</param>
        // <returns>название файла + спаршеные данные</returns>
        public async Task<string> CreateProcessingParsing(string path)
        {
            ServiceParsingPdf temp_obj = new ServiceParsingPdf();

            return $"{Path.GetFileName(path)} " + await temp_obj.proccessing(path, count_pipe++);
        }
    }
}
