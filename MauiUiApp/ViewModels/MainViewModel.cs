using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiUiApp.repository;
using MauiUiApp.service;
using MauiUiApp.serviceFactory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;

namespace MauiUiApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        private string selectedPath = "Выберите файл или папку...";

        [ObservableProperty]
        private string colorText = "Gray";

        [ObservableProperty]
        private double _progressValue;

        [ObservableProperty]
        private bool _showProgress = false;

        // <summary>
        // Класс BlockModel нужен для динамического добавление (по шаблону) и вывода спаршеных данных из PDF
        // </summary>
        [ObservableProperty]
        private ObservableCollection<BlockModel> blocks = new();

        private double progress_step = 0;

        List<string> paths;

        // <summary>
        // Сохраняет изменение
        // </summary>
        private void SaveData(List<string> result) {
            paths = result ?? paths;
            ColorText = paths != null ? "Black" : "Grey";
            SelectedPath = paths != null ? $"{Path.GetDirectoryName(paths[0])}..." : "Выберите файл или папку...";
        }

        // <summary>
        // Функция для открытия проводника для выбора PDF файлов
        // </summary>
        [RelayCommand]
        private async Task BrowseButtonFile()
        {
            IBrowseButton creator = new BrowseButtonFile();

            var result = await creator.PickFile();

            SaveData(result);
        }

        // <summary>
        // Функция для открытия проводника для выбора папки с PDF файломи
        // </summary>
        [RelayCommand]
        private async Task BrowseButtonFolder()
        {
            IBrowseButton creator = new BrowseButtonFolder();

            var result = await creator.PickFile();

            SaveData(result);
        }

        // <summary>
        // Функция для получения нужных данных из PDF файлов.
        // Создаем экземпляр фабрики IServiceFactory для парсинка pdf serviceFactoryForPDF
        // </summary>
        [RelayCommand]
        private async Task GetData()
        {
            var tasks = new List<Task<string>>();

            IServiceFactory creator = new serviceFactoryForPDF();

            if (paths !=null)
            {
                Blocks.Clear();
                ProgressValue = 0;
                progress_step = 1.0 / (paths.Count() - 1);
                ShowProgress = true;

                foreach (string path in paths)
                {
                    tasks.Add(creator.CreateProcessingParsing(path));
                }

                while (tasks.Any())
                {
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);

                    var result = await completedTask;
                    
                    await ButtonBlocks(result.Split(' ')[0], result.Split(' ')[1], result.Split(' ')[2]);
                    ProgressValue += progress_step;
                }
            }
        }

        // <summary>
        // Создает блок (по шаблону) из полученой информации из PDF
        // </summary>
        // <param name="PdfFileName">Имя PDF файла</param>
        // <param name="Result1">Текст, который будет подставлен в шаблон (Количество строк)</param>
        // <param name="Result2">Текст, который будет подставлен в шаблон (Количество bounding-box)</param>
        private async Task ButtonBlocks(string PdfFileName, string Result1, string Result2)
        {
            var newBlock = new BlockModel
            {
                PdfFileName = PdfFileName,
                Result1 = Result1,
                Result2 = Result2
            };

            blocks.Add(newBlock);
        }
    }
}
