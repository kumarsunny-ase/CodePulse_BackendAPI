using System;
using CodePulse.Models.Domain;

namespace CodePulse.Repositories.Interface
{
	public interface IImageRepository
	{
		Task<BlogPostImage> Upload(IFormFile file, BlogPostImage blogPostImage);

		Task<IEnumerable<BlogPostImage>> GetAll();
	}
}

