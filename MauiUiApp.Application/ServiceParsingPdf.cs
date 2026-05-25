using MauiUiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.Application
{
    public class ServiceParsingPdf
    {
        public async Task<string> proccessing(string path, int number_pipe)
        {
            return await PipeFileRepository.processing(path, number_pipe);
        }
    }
}
