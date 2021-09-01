using System;
using AutoMapper;
using HxFood.Services.Catalog.Dtos.Product;
using HxFood.Services.Catalog.Models;
using HxFood.Services.Catalog.Services.Interfaces;
using HxFood.Services.Catalog.Settings;
using HxFood.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace HxFood.Services.Catalog.Services
{
    public class ProductService : IProductService
    {
        #region Veriables

        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Cache Options
        private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(5)
        };
        #endregion

        #region Constructor

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, IDistributedCache distributedCache)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        #endregion                                  

        #region Crud Operations

        public async Task<Response<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productCollection.Find(product => true).ToListAsync();
            
            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.Category = await _categoryCollection.Find<Category>(cat => cat.Id == product.CategoryId)
                        .FirstAsync();
                }
            }
            else
            {
                products = new List<Product>();
            }

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
        }

        public async Task<Response<ProductDto>> GetByIdAsync(string id)
        {
            var existProduct = await _distributedCache.GetStringAsync(id);

            if (string.IsNullOrEmpty(existProduct))
            {
                var product = await _productCollection.Find(product => product.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return Response<ProductDto>.Fail("Product not found", 404);
                }

                product.Category =
                    await _categoryCollection.Find<Category>(cat => cat.Id == product.CategoryId).FirstAsync();

                await _distributedCache.SetStringAsync(product.Id, JsonSerializer.Serialize(product), _cacheOptions);

                return Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), 200);
            }

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(JsonSerializer.Deserialize<Product>(existProduct)), 200);
        }

        public async Task<Response<ProductDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var newProduct = _mapper.Map<Product>(productCreateDto);
            await _productCollection.InsertOneAsync(newProduct);
            await _distributedCache.SetStringAsync(newProduct.Id, JsonSerializer.Serialize(newProduct), _cacheOptions);
            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 201);
        }

        public async Task<Response<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(productUpdateDto);
            var result =
                await _productCollection.FindOneAndReplaceAsync(x => x.Id == productUpdateDto.Id, updateProduct);

            if (result == null)
            {
                return Response<NoContent>.Fail("Product not found", 404);
            }

            await _distributedCache.SetStringAsync(result.Id, JsonSerializer.Serialize(result), _cacheOptions);

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _productCollection.DeleteOneAsync(product => product.Id == id);
            if (result.DeletedCount > 0)
            {
                await _distributedCache.RemoveAsync(id);
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Product not found", 404);
        }


        #endregion

    }
}
