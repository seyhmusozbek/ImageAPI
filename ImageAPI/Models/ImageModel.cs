using Microsoft.AspNetCore.Http;

namespace ImageAPI.Models
{
    public class ImageModel
    {
        public ImageDetailDTO imgDetDTO { get; set; }
        public IFormFile file { get; set; }
    }
}
