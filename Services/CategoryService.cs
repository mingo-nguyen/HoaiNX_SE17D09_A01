﻿using Domain;
using Repository;
using Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(short id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        await _categoryRepository.UpdateCategoryAsync(category);
    }

    public async Task DeleteCategoryAsync(short id)
    {
        await _categoryRepository.DeleteCategoryAsync(id);
    }
}
