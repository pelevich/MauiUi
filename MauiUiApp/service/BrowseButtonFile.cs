using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.service
{
    public class BrowseButtonFile : IBrowseButton
    {
        public async Task<List<string>> PickFile()
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Выберите",
                FileTypes = FilePickerFileType.Pdf
            });

            if (result != null && result.Any()) { 

                return result.Select(f => f.FullPath).ToList();

            }

                return null;
        }

    }
}
