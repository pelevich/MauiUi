using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Storage;
using MauiUiApp.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        /// <summary>
        /// Класс BlockModel нужен для динамического добавление (по шаблону) и вывода спаршеных данных из PDF
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<BlockModel> blocks = new();

        private double progress_step = 0;

        List<string> paths;

        /// <summary>
        /// Функция для открытия проводника для выбора PDF файлов
        /// </summary>
        [RelayCommand]
        private async Task BrowseButtonFile()
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Выберите",
                FileTypes = FilePickerFileType.Pdf
            });

            if (result != null && result.Any())
            {
                ColorText = "Black";
                SelectedPath = $"{Path.GetDirectoryName(result.First().FullPath)}...";
                paths = result.Select(f => f.FullPath).ToList();
            }
        }

        /// <summary>
        /// Функция для открытия проводника для выбора папки с PDF файломи
        /// </summary>
        [RelayCommand]
        private async Task BrowseButtonFolder()
        {
            var result = await FolderPicker.Default.PickAsync(CancellationToken.None);

            if (result.IsSuccessful)
            {
                ColorText = "Black";
                SelectedPath = $"{result.Folder.Path}...";
                paths = Directory.GetFiles(result.Folder.Path, "*.pdf", SearchOption.TopDirectoryOnly).ToList();
            }
        }

        /// <summary>
        /// Функция для получения нужных данных из PDF файлов.
        /// Для каждого файла вызывается отдельный асинхроный процесс.
        /// </summary>
        [RelayCommand]
        private async Task GetData()
        {
            var tasks = new List<Task>();
            var count_pipe = 1;
            if(paths !=null)
            {
                foreach (string path in paths)
                {
                    tasks.Add(processing(path, count_pipe++));
                }

                Blocks.Clear();
                ProgressValue = 0;
                progress_step = 1.0 / (count_pipe - 1);
                ShowProgress = true;
            }
        }

        // <summary>
        // Вызываем бэкенд для получение информации из PDF
        // </summary>
        // <param name="path">Ссылка на файл</param>
        // <param name="number_pipe">Номер пайпа для подключения</param>
        private async Task processing(string path, int number_pipe)
        {
            MyPipe pipe = new MyPipe();
            await pipe.InstancPipe("Pipe_number_" + number_pipe.ToString());

            string publishDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string appPath = Path.Combine(publishDirectory, "MauiApp", "MauiApp.exe");

            string arg1 = path;
            string arg2 = "Pipe_number_" + number_pipe.ToString();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = appPath,
                Arguments = $"{arg1} {arg2}",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            Process process = Process.Start(startInfo);

            await pipe.server.WaitForConnectionAsync();
            await pipe.ReadMessage();
            await ButtonBlocks(Path.GetFileName(path), pipe.receivedMessage.Split(' ')[0], pipe.receivedMessage.Split(' ')[1]);
            ProgressValue += progress_step;
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
