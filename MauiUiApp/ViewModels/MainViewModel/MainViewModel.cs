using ApplicationService.Service;
using ApplicationService.Service.serviceFactory;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiUiApp.ViewModels.MainViewModel
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly IServiceFactory _creator;
        private readonly IBrowseButton _browseButtonFile;
        private readonly IBrowseButton _browseButtonFolder;

        public MainViewModel(IServiceFactory ServiceFactory, IEnumerable<IBrowseButton> BrowseButton)
        {
            _creator = ServiceFactory;
            var buttons = BrowseButton.ToList();
            _browseButtonFile = buttons[0];
            _browseButtonFolder = buttons[1];
        }

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
            var result = await _browseButtonFile.PickFile();

            SaveData(result);
        }

        // <summary>
        // Функция для открытия проводника для выбора папки с PDF файломи
        // </summary>
        [RelayCommand]
        private async Task BrowseButtonFolder()
        {
            var result = await _browseButtonFolder.PickFile();

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

            if (paths !=null)
            {
                Blocks.Clear();
                ProgressValue = 0;
                progress_step = 1.0 / (paths.Count() - 1);
                ShowProgress = true;

                foreach (string path in paths)
                {
                    var dto = await FileUploadDto.FromPathAsync(path);
                    tasks.Add(_creator.CreateProcessingParsing(dto.FileData, dto.Length));
                }

                while (tasks.Any())
                {
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);

                    var result = await completedTask;
                    
                    string fileName = "blank";
                    string arg1 = result.Split(' ')[0];
                    string arg2 = result.Split(' ')[1];

                    await ButtonBlocks(fileName, arg1, arg2);
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
