using System.Collections.Generic;
using System.Net;
using HxFood.Services.Catalog.Dtos.Product;
using HxFood.Services.Catalog.Services.Interfaces;
using HxFood.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HxFood.Shared.Dtos;

namespace HxFood.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomBaseController
    {
        #region Veriables
        private readonly IProductService _productService;
        #endregion

        #region Constructor
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Crud Operations
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(List<Response<ProductDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [ProducesResponseType(typeof(Response<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _productService.GetByIdAsync(id);
            return CreateActionResultInstance(response);
        }

        /// <summary>
        /// Create an Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST api/products/create
        ///{
        ///  "id": "612a08f3fa311c1dde582591",
        ///  "name": "Kayseri Mantısı",
        ///  "description": "Kayseri mantısı,yoğurt ve domates sosu.",
        ///  "categoryId": "categoryId",
        ///  "price": 35,
        ///  "currency": "TL",
        ///}
        /// </remarks>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            var response = await _productService.CreateAsync(productCreateDto);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            var response = await _productService.UpdateAsync(productUpdateDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _productService.DeleteAsync(id);
            return CreateActionResultInstance(response);
        }
        #endregion
    }
}
