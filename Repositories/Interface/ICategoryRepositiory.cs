using System;
using CodePulse.Models.Domain;

namespace CodePulse.Repositories
{
	public interface ICategoryRepositiory 
	{
		Task<Category> CreateAsync(Category category);

		Task<IEnumerable<Category>> GetAllAsync();

		Task<Category?> GetById(Guid id);

		Task<Category?> UpdateAsync(Category category);

		Task<Category?> DeleteAsync(Guid id);
	}
}

