using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.Application.serviceFactory
{
    public interface IServiceFactory
    {
        public Task<string> CreateProcessingParsing(string path);
    }
}
