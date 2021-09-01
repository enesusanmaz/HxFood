using AutoMapper;
using HxFood.Services.Catalog.Dtos.Category;
using HxFood.Services.Catalog.Models;
using HxFood.Services.Catalog.Services.Interfaces;
using HxFood.Services.Catalog.Settings;
using HxFood.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HxFood.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        #region Veriables

        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        #endregion

        #region Crud Operations

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(cat => true).ToListAsync();
            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }
        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(cat => cat.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
        public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var newCategory = _mapper.Map<Category>(categoryCreateDto);
            await _categoryCollection.InsertOneAsync(newCategory);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(newCategory),201);
        }
        public async Task<Response<NoContent>> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var updateCategory = _mapper.Map<Category>(categoryUpdateDto);
            var result =
                await _categoryCollection.FindOneAndReplaceAsync(x => x.Id == categoryUpdateDto.Id, updateCategory);

            if (result == null)
            {
                return Response<NoContent>.Fail("Category not found",404);
            }

            return Response<NoContent>.Success(204);
        }
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
           var result = await _categoryCollection.DeleteOneAsync(cat => cat.Id == id);
           if (result.DeletedCount>0)
           {
               return Response<NoContent>.Success(204);
           }
           return Response<NoContent>.Fail("Category not found",404);
        }

        #endregion
    }
}
