using System;
using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementation
{
	public class BlogPostRepository : IBlogPostRepository
	{
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogPostRepository(ApplicationDbContext applicationDbContext)
		{
            _applicationDbContext = applicationDbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _applicationDbContext.BlogPosts.AddAsync(blogPost);
            await _applicationDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAync(Guid id)
        {
           var existingBlogPost = await _applicationDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if(existingBlogPost != null)
            {
                _applicationDbContext.BlogPosts.Remove(existingBlogPost);
                await _applicationDbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _applicationDbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost> GetById(Guid id)
        {
           return await _applicationDbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandle(string urlHandle)
        {
            return await _applicationDbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _applicationDbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost == null)
            {
                return null;
            }

            // Update BlogPost Database

            _applicationDbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await _applicationDbContext.SaveChangesAsync();

            return blogPost;

        }
    }
}

