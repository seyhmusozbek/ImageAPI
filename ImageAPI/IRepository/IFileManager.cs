using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ImageAPI.IRepository
{
    public interface IFileManager
    {
        Task<bool> SaveFile(IFormFile file,string directory,string id);
        Task DeleteFile(string id);
        Task<MemoryStream> AdjustFile(Stream image, float value);
        Task<MemoryStream> ResizeFile(string directory, int width, int height);

    }
}
