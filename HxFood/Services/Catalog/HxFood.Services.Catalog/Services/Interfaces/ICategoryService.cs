using HxFood.Services.Catalog.Dtos.Category;
using HxFood.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HxFood.Services.Catalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
        Task<Response<NoContent>> DeleteAsync(string id);
        Task<Response<NoContent>> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
    }
}
