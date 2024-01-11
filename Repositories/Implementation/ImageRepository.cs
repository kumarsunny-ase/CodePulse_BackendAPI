using System;
using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementation
{
	public class ImageRepository : IImageRepository
	{
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
		{
            this.webHostEnvironment = webHostEnvironment;
           _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<BlogPostImage>> GetAll()
        {
            return await _applicationDbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogPostImage> Upload(IFormFile file, BlogPostImage blogPostImage)
        {
            //step-1 - Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogPostImage.FileName}{blogPostImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //step-2 - Update the database
            //https://codepulse.com/images/somefilename.jpg

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogPostImage.FileName}{blogPostImage.FileExtension}";

            blogPostImage.Url = urlPath;

            await _applicationDbContext.BlogImages.AddAsync(blogPostImage);
            await _applicationDbContext.SaveChangesAsync();

            return blogPostImage;
        }
    }
}

