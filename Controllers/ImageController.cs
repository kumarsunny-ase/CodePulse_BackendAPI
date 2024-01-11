using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodePulse.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if(ModelState.IsValid)
            {
                var blogImage = new BlogPostImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                var recievedBlogImage = await _imageRepository.Upload(file, blogImage);

                BlogPostImageDto response = new BlogPostImageDto
                {
                    Id = recievedBlogImage.Id,
                    Title = recievedBlogImage.Title,
                    DateCreated = recievedBlogImage.DateCreated,
                    FileExtension = recievedBlogImage.FileExtension,
                    FileName = recievedBlogImage.FileName,
                    Url = recievedBlogImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 10 MB");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();

            var response = new List<BlogPostImageDto>();

            foreach (var image in images)
            {
                response.Add(new BlogPostImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }
            return Ok(response);
        }
    }
}

