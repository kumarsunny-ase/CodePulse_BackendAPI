using System;
using Azure.Core;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories;
using CodePulse.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.Controllers
{
    [Route("api/BlogPost")]
    [ApiController]
    public class BlogPostController : ControllerBase
	{
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepositiory _categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepositiory categoryRepository)
		{
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

		[HttpPost]
		public async Task<IActionResult> CreateBlogPost(CreateBlogpostRequestDto request)
		{
			var blogPost = new BlogPost
			{
				Title = request.Title,
				Author = request.Author,
				Content = request.Content,
                publishedDate = request.publishedDate,
				UrlHandle = request.UrlHandle,
				IsVisible = request.IsVisible,
				ShortDescription = request.ShortDescription,
				FeaturedImageUrl = request.FeaturedImageUrl,
				Categories = new List<Category>()
			};

			foreach (var categoryGuid in request.Categories)
			{
				var existingCategory = await _categoryRepository.GetById(categoryGuid);
				if(existingCategory is not null)
				{
					blogPost.Categories.Add(existingCategory);
				}
			}

			blogPost = await _blogPostRepository.CreateAsync(blogPost);

			var response = new BlogPostDto
			{
				Id = blogPost.Id,
				Author = blogPost.Author,
				IsVisible = blogPost.IsVisible,
				Title = blogPost.Title,
                publishedDate = blogPost.publishedDate,
				ShortDescription = blogPost.ShortDescription,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				Content = blogPost.Content,
				UrlHandle = blogPost.UrlHandle,
				Categories = blogPost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()

			};
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetALlBlogPost()
		{
			var blogposts = await _blogPostRepository.GetAllAsync();

			// convert domain model to DTO
			var response = new List<BlogPostDto>();
			foreach (var blogPost in blogposts)
			{
				response.Add(new BlogPostDto
				{
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    IsVisible = blogPost.IsVisible,
                    Title = blogPost.Title,
                    publishedDate = blogPost.publishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
			}
			return Ok(response);
		}

		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
		{
			var blogPost = await _blogPostRepository.GetById(id);
			if (blogPost is null)
			{
				return NotFound();
			}

			var response = new BlogPostDto
			{
				Id = blogPost.Id,
				Author = blogPost.Author,
				IsVisible = blogPost.IsVisible,
				Title = blogPost.Title,
                publishedDate = blogPost.publishedDate,
				ShortDescription = blogPost.ShortDescription,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				Content = blogPost.Content,
				UrlHandle = blogPost.UrlHandle,
				Categories = blogPost.Categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
					UrlHandle = x.UrlHandle
				}).ToList()
			};
			return Ok(response); 
        }

		[HttpGet]
		[Route("{urlHandle}")]
		public async Task<IActionResult> GetBlogPostUrlHandle([FromRoute] string urlHandle)
		{
			var blogPost = await _blogPostRepository.GetByUrlHandle(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Title = blogPost.Title,
                publishedDate = blogPost.publishedDate,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);


        }

		[HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdatedBlogPostRequestDto request)
		{
			var blogPost = new BlogPost
			{
				Id = id,
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                publishedDate = request.publishedDate,
                UrlHandle = request.UrlHandle,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                Categories = new List<Category>()
            };

			foreach (var categoryGuid in request.Categories)
			{
				var existingCategory = await _categoryRepository.GetById(categoryGuid);

				if (existingCategory != null)
				{
					blogPost.Categories.Add(existingCategory);
				}				
            }

            var updateBlogPost = await _blogPostRepository.UpdateAsync(blogPost);

            if (updateBlogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Title = blogPost.Title,
                publishedDate = blogPost.publishedDate,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			var deletedBlogPost = await _blogPostRepository.DeleteAync(id);

			if(deletedBlogPost == null)
			{
				return NotFound();
			}

			var response = new BlogPostDto
			{
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                IsVisible = deletedBlogPost.IsVisible,
                Title = deletedBlogPost.Title,
                publishedDate = deletedBlogPost.publishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                Content = deletedBlogPost.Content,
                UrlHandle = deletedBlogPost.UrlHandle
            };

			return Ok(response);
		}
	}
}

