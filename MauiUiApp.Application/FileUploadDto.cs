using System;
using System.Collections.Generic;
using System.Text;

namespace MauiUiApp.Application
{
    public class FileUploadDto
    {
        public byte[] FileData { get; private set; }
        public string FileName { get; private set; }
        public string fileFieldName { get; private set; }
        public int Length { get; private set; }

        // <summary>
        //DTO для файлов от UI к Api
        // </summary>
        // <param name="filePath">путь до обрабатываемого файла</param>
        // <return>обработаный объект</param>
        public static async Task<FileUploadDto> FromPathAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            byte[] fileData = await File.ReadAllBytesAsync(filePath);

            var dto = new FileUploadDto
            {
                FileName = Path.GetFileName(filePath),
                FileData = fileData,
                fileFieldName = "file",
                Length = fileData.Length
            };

            return dto;
        }
    }
}
