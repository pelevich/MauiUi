using MauiUiApp.repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MauiUiApp.service
{
    public class ServiceParsingPdf
    {
        public async Task<string> proccessing(string path, int number_pipe)
        {
            return await PipeFileRepository.processing(path, number_pipe);
        }
    }
}
