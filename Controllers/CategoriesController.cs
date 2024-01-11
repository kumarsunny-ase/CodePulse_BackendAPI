using System;
using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.Controllers
{
	[Route("api/Categories")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepositiory _categoryRepositiory;

		public CategoriesController(ICategoryRepositiory categoryRepositiory)
		{
			_categoryRepositiory = categoryRepositiory;
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
		{
			var category = new Category
			{
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};

			await _categoryRepositiory.CreateAsync(category);

			var response = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};

			return Ok(response);
		}

		[HttpGet]

		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryRepositiory.GetAllAsync();

			var response = new List<CategoryDto>();
			foreach (var category in categories)
			{
				response.Add(new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
					UrlHandle = category.UrlHandle
				});
			}
			return Ok(response);
		}

		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
		{
			var existingCategory = await _categoryRepositiory.GetById(id);

			if (existingCategory == null)
			{
				return NotFound();
			}

			var response = new CategoryDto
			{
				Id = existingCategory.Id,
				Name = existingCategory.Name,
				UrlHandle = existingCategory.UrlHandle
			};
			return Ok(response);
		}

		[HttpPut]
		[Route("{id:Guid}")]
		public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
		{
			var category = new Category
			{
				Id = id,
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};

			category = await _categoryRepositiory.UpdateAsync(category);

			if (category == null)
			{
				return NotFound();
			}

			var response = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};
			return Ok(response);
		}

		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
		{
			var category = await _categoryRepositiory.DeleteAsync(id);
			if(category == null)
			{
				return NotFound();
			}

			var response = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};
			return Ok(response);
		}
	}
}

