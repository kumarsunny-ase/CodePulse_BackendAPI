﻿using System;
using CodePulse.Data;
using CodePulse.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Interface
{
    public class CategoryRepositiory : ICategoryRepositiory
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepositiory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCategory == null)
            {
                return null;
            }

            _dbContext.Categories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync();
            return existingCategory;
           
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if(existingCategory != null)
            {
                _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await _dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
