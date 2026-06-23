using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.ViewModels
{
    public interface IBrowseButton
    {
        public Task<List<string>> PickFile();
    }
}
