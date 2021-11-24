using ImageAPI.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace ImageAPI.Repository
{
    public class FileManager : IFileManager
    {
        public async Task<MemoryStream> AdjustFile(Stream rawImage, float value)
        {
            Image image = Image.Load(rawImage);
            image.Mutate(x => x
            .Brightness(value)
            .Contrast(value));
            var adjustedImage = new MemoryStream();
            await image.SaveAsPngAsync(adjustedImage);
            return adjustedImage;
        }

        public Task DeleteFile(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<MemoryStream> ResizeFile(string directory, int width, int height)
        {
            byte[] inputStream = File.ReadAllBytes(directory);
            using var image = Image.Load(inputStream);
            var outputStream = new MemoryStream();
            image.Mutate(x => x.Resize(width, height));
            await image.SaveAsPngAsync(outputStream);
            outputStream.Position = 0;
            return outputStream;
        }

        public async Task<bool> SaveFile(IFormFile file, string directory, string id)
        {

            string extension = Path.GetExtension(file.FileName);
            extension = extension.ToLower();
            if (!extension.Equals(".png", StringComparison.CurrentCultureIgnoreCase))
                return false;

            if (file.Length > 0)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using FileStream fStream = File.Create(Path.Combine (directory,id + extension));
                Log.Logger.Information("length of file " + file.Length.ToString());
                var adjustedImage = await AdjustFile(file.OpenReadStream(),50);
                Log.Logger.Information("length of adjusted image "+adjustedImage.Length.ToString());
                adjustedImage.Seek(0,SeekOrigin.Begin);
                await adjustedImage.CopyToAsync(fStream);
                Log.Logger.Information("length of adjusted fstream " + fStream.Length.ToString());
                await fStream.FlushAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
