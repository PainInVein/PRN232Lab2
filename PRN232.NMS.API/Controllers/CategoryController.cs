using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.API.Models.RequestModels.CategoryRequests;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.API.Models.ResponseModels.CategoryResponses;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.API.Controllers
{
    /// <summary>
    /// RESTful Category API. Resource-based: /api/categories, /api/categories/{id}.
    /// Query: searchTerm, isActive, parentCategoryId, sortBy, sortOrder, page, pageSize, fields.
    /// </summary>
    [Route("api/categories")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// GET Collection. Search, filter, sort, paging, field selection.
        /// Query: searchTerm, isActive, parentCategoryId, sortBy, sortOrder, page, pageSize, fields (minimal | full).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterRequest filter)
        {
            var page = Math.Max(1, filter.Page);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var (items, totalItems) = await _categoryService.GetCategoriesPagedAsync(
                page,
                pageSize,
                filter.SearchTerm,
                filter.IsActive,
                filter.ParentCategoryId,
                filter.SortBy,
                filter.SortOrder);

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            object pagedData;
            if (string.Equals(filter.Fields, "minimal", StringComparison.OrdinalIgnoreCase))
            {
                var minimal = _mapper.Map<List<CategoryMinimalResponse>>(items);
                pagedData = new PagedResult<CategoryMinimalResponse>
                {
                    Items = minimal,
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
            }
            else
            {
                var full = _mapper.Map<List<GetAllResponse>>(items);
                pagedData = new PagedResult<GetAllResponse>
                {
                    Items = full,
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
            }

            var response = new ResponseDTO<object>("Categories retrieved successfully", true, pagedData, null);
            return Ok(response);
        }

        /// <summary>
        /// GET by ID. Full related info (ParentCategoryName, ChildCategories). No circular ref.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ResponseDTO<GetByIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO<GetByIdResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new ResponseDTO<GetByIdResponse>("Category not found.", false, null, null));

            var mapped = _mapper.Map<GetByIdResponse>(category);
            return Ok(new ResponseDTO<GetByIdResponse>("Category retrieved successfully", true, mapped, null));
        }

        /// <summary>
        /// POST Create. Validates via CreateCategoryRequest.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            await _categoryService.CreateAsync(entity);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseDTO<object>("Category created successfully", true, null, null));
        }

        /// <summary>
        /// PUT Update. Full update (replace). Validates circular reference, parent existence.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            var result = await _categoryService.UpdateAsync(id, entity);

            if (result != string.Empty)
                return NotFound(new ResponseDTO<object>($"Category update failed: {result}", false, null, null));

            return Ok(new ResponseDTO<object>("Category updated successfully", true, null, null));
        }

        /// <summary>
        /// DELETE. Cannot delete if has NewsArticles or child categories.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result != string.Empty)
                return NotFound(new ResponseDTO<object>($"Category deletion failed: {result}", false, null, null));

            return Ok(new ResponseDTO<object>("Category deleted successfully", true, null, null));
        }
    }
}
