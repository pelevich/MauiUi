using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.ViewModels
{
    internal interface IBrowseButton
    {
        public Task<List<string>> PickFile();
    }
}
