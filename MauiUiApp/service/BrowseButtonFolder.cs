using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.service
{
    public class BrowseButtonFolder : IBrowseButton
    {
        public async Task<List<string>> PickFile()
        {
            var result = await FolderPicker.Default.PickAsync(CancellationToken.None);

            if (result.IsSuccessful)
            {
                return Directory.GetFiles(result.Folder.Path, "*.pdf", SearchOption.TopDirectoryOnly).ToList();
            }

            return null;
        }
    }
}
