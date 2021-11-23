using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Models
{
    public class ImageDetailDTO
    {
        [Required]
        public string fileType { get; set; }
        public string filter { get; set; }
        [Required]
        public string colorType { get; set; }
        [Required]
        public string imageSize { get; set; }
        [Required]
        public int bitDepth { get; set; }
        [Required]
        public double gamma { get; set; }
    }
}
