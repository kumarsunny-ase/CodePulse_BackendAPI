using System;
using CodePulse.Models.Domain;

namespace CodePulse.Repositories.Interface
{
	public interface IBlogPostRepository
	{
		Task<BlogPost> CreateAsync(BlogPost blogPost);

		Task<IEnumerable<BlogPost>> GetAllAsync();

		Task<BlogPost> GetById(Guid id);

		Task<BlogPost?> GetByUrlHandle(string urlHandle);

		Task<BlogPost?> UpdateAsync(BlogPost blogPost);

		Task<BlogPost?> DeleteAync(Guid id);
	}
}

