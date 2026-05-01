using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.serviceFactory
{
    internal interface IServiceFactory
    {
        public Task<string> CreateProcessingParsing(string path);
    }
}
