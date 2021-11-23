using AutoMapper;
using ImageAPI.Entities;
using ImageAPI.IRepository;
using ImageAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public static IUnitOfWork _unitOfWork;
        public static IFileManager _fileManager;
        private readonly ILogger<ImageController> _iLogger;
        private readonly IMapper _mapper;


        public ImageController(IWebHostEnvironment environment, IUnitOfWork unitOfWork, IFileManager fileManager, ILogger<ImageController> iLogger, IMapper mapper)
        {
            _environment = environment;
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _iLogger = iLogger;
            _mapper = mapper;
        }

        [HttpGet(Name ="GetAll")]
        public async Task<IActionResult> GetAllMetaData()
        {
            return Ok(await _unitOfWork.image.GetAll());
        }
        [HttpGet("{imgType}/{id}", Name = "thumbnail")]
        public async Task<IActionResult> GetAsType(string imgType, string id)
        {
            try
            {
                if (imgType!="thumbnail" && imgType!="small" && imgType!="large")
                {
                    _iLogger.LogInformation($"Attemption to get the file failed because of image type. Type must be one of thumbnail, small, large!");
                    return BadRequest($"There is no type such as {imgType}. Type must be one of thumbnail, small, large!");
                }
                int width=0;
                int height=0;
                switch (imgType)
                {
                    case "thumbnail":
                        width = 128;
                        height = 128;
                        break;
                    case "small":
                        width = 512;
                        height = 512;
                        break;
                    case "large":
                        width = 2048;
                        height = 2048;
                        break;
                }
                _iLogger.LogInformation($"Attempted to get the file id: {id}!");
                string directory = _environment.WebRootPath + "/Upload/" + id + ".png";
                var outputStream = await _fileManager.ResizeFile(directory, width, height);
                var imgData = await _unitOfWork.image.Get(a => a.id == id);

                return File(outputStream, "application/octet-stream", imgData.fileName);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, "Failed to get the image!");
                return BadRequest("Failed to get the image! Please check the logs.");
            }
        }

        [HttpGet("{id}/{width}/{height}", Name = "GetAsSized")]
        public async Task<IActionResult> GetImageAsSized(string id, int width, int height)
        {
            try
            {
                _iLogger.LogInformation($"Attempted to get the file id: {id}!");
                if (width < 100 || width > 2048 || height < 100 || height > 2048)
                {
                    _iLogger.LogInformation($"Attemption to get the file failed because of size, id: {id}!");
                    return BadRequest("The height and width properties must be between 100 and 2048");
                }

                string directory = _environment.WebRootPath + "/Upload/" + id + ".png";
                var outputStream = await _fileManager.ResizeFile(directory,width,height);
                var imgData = await _unitOfWork.image.Get(a => a.id == id);

                return File(outputStream, "application/octet-stream", imgData.fileName);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, "Failed to get the image!");
                return BadRequest("Failed to get the image! Please check the logs.");
            }
        }

        [HttpGet("{id}",Name ="GetMetaData")]
        public async Task<IActionResult> GetMetaData(string id)
        {
            try
            {
                var imgData = await _unitOfWork.image.Get(a => a.id == id);
                if (imgData == null)
                    return BadRequest($"There is no such metadata on id {id}!");
                return Ok(imgData);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, "Failed to get the metadata!");
                return BadRequest("Failed to get the metadata! Please check the logs.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] ImageModel imgModel)
        {
            try
            {
                _iLogger.LogInformation($"Attempted to save the file named {imgModel.file.FileName}!");

                var image = _mapper.Map<ImageDetail>(imgModel.imgDetDTO);
                image.fileName = imgModel.file.FileName;
                image.saveDate = DateTimeOffset.Now;

                await _unitOfWork.image.InsertOne(image);
                bool isSaved = await _fileManager.SaveFile(imgModel.file, _environment.WebRootPath, image.id);
                return Ok(new { img = image, isSaved = isSaved });
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, "Failed to save the image!");
                return BadRequest("Failed to save the image! Please check the logs.");
            }

        }
    }
}
