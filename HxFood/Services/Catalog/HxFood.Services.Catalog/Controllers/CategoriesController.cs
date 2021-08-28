using System.Collections.Generic;
using System.Net;
using HxFood.Services.Catalog.Dtos.Category;
using HxFood.Services.Catalog.Services.Interfaces;
using HxFood.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HxFood.Shared.Dtos;

namespace HxFood.Services.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : CustomBaseController
    {
        #region Veriables

        private readonly ICategoryService _categoryService;

        #endregion

        #region Constructor

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        #region Crud Operations
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(List<Response<CategoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [ProducesResponseType(typeof(Response<CategoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(response);
        }

        /// <summary>
        /// Create an Category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST api/categories/create
        /// 
        ///{
        ///  "name": "Türk Mutfağı",
        ///  "description": "Türk mutfağı, Türkiye'nin ulusal mutfağıdır. Osmanlı kültürünün mirasçısı olan Türk mutfağı hem Balkan ve Orta Doğu mutfaklarını etkilemiş hem de bu mutfaklardan etkilenmiştir. Ayrıca Türk mutfağı yörelere göre de farklılıklar gösterir."
        ///}
        /// </remarks>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Create(CategoryCreateDto productCreateDto)
        {
            var response = await _categoryService.CreateAsync(productCreateDto);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(CategoryUpdateDto productUpdateDto)
        {
            var response = await _categoryService.UpdateAsync(productUpdateDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(Response<NoContent>), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _categoryService.DeleteAsync(id);
            return CreateActionResultInstance(response);
        }

        #endregion

    }
}
