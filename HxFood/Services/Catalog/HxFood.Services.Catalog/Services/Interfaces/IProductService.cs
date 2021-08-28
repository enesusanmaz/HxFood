using HxFood.Services.Catalog.Dtos.Product;
using HxFood.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HxFood.Services.Catalog.Services.Interfaces
{
    public interface IProductService
    {
        Task<Response<List<ProductDto>>> GetAllAsync();
        Task<Response<ProductDto>> GetByIdAsync(string id);
        Task<Response<ProductDto>> CreateAsync(ProductCreateDto productCreateDto);
        Task<Response<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
