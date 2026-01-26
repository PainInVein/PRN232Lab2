using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using PRN232.NMS.Services.Models.RequestModels.NewsArticleRequests;
using PRN232.NMS.Services.Models.ResponseModels;
using PRN232.NMS.Services.Models.ResponseModels.NewsArticleResponse;
using System.Security.Claims;

namespace PRN232.NMS.API.Controllers
{
    [Route("api/newsarticles")]
    [ApiController]
    [AllowAnonymous]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _newsService;
        private readonly IMapper _mapper;

        public NewsArticleController(INewsArticleService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] NewsArticleFilterRequest request)
        {
            var result = await _newsService.GetAllPagedAsync(
                request.SearchTerm,
                request.CategoryId,
                request.NewsStatusId,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize);

            var mappedItems = _mapper.Map<List<NewsArticleResponse>>(result.Items);

            var pagedResponse = new PagedResult<NewsArticleResponse>
            {
                Items = mappedItems,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = (int)Math.Ceiling(result.TotalItems / (double)request.PageSize)
            };

            return Ok(new ResponseDTO<PagedResult<NewsArticleResponse>>("Success", true, pagedResponse, null));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _newsService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound(new ResponseDTO<object>("News Article not found", false, null, "Resource not found"));
            }

            var response = _mapper.Map<NewsArticleResponse>(article);
            return Ok(new ResponseDTO<NewsArticleResponse>("Success", true, response, null));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO<object>("Validation failed", false, null, "Invalid input data"));
            }

            var entity = _mapper.Map<NewsArticle>(request);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("Id")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                entity.CreatedById = userId;
            }
            else
            {
                entity.CreatedById = 1;
            }

            await _newsService.CreateAsync(entity, request.TagIds);

            return StatusCode(201, new ResponseDTO<object>("Article created successfully", true, null, null));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO<object>("Validation failed", false, null, "Invalid input data"));
            }

            var entity = _mapper.Map<NewsArticle>(request);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("Id")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                entity.UpdatedById = userId;
            }

            await _newsService.UpdateAsync(id, entity, request.TagIds);

            return Ok(new ResponseDTO<object>("Article updated successfully", true, null, null));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.DeleteAsync(id);

            return Ok(new ResponseDTO<object>("Article deleted successfully", true, null, null));
        }
    }
}