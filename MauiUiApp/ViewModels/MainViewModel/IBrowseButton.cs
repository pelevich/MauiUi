using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.ViewModels.MainViewModel
{
    public interface IBrowseButton
    {
        public Task<List<string>> PickFile();
    }
}
