using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.API.Models.RequestModels;
using PRN232.NMS.API.Models.RequestModels.NewsArticleRequests;
using PRN232.NMS.API.Models.RequestModels.TagRequests;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.API.Models.ResponseModels.TagResponses;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using System.Security.Claims;

namespace PRN232.NMS.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly IModelStateCheck _modelStateCheck;

        public TagController(ITagService tagService, IMapper mapper, IModelStateCheck modelStateCheck)
        {
            _tagService = tagService;
            _mapper = mapper;
            _modelStateCheck = modelStateCheck;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag != null)
            {
                var mappedTag = _mapper.Map<GetByIdResponse>(tag);
                var response = new ResponseDTO<GetByIdResponse>(message: "Tag retrieved successfully", isSuccess: true, data: mappedTag, errors: null);
                return Ok(response);
            }
            return NotFound(new ResponseDTO<GetByIdResponse>(message: "Tag not found", isSuccess: false, data: null, errors: null));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags([FromQuery] TagFilterRequest tagFilterRequest)
        {
            var pagedTags = await _tagService.GetTagsPagedAsync(tagFilterRequest.Page, tagFilterRequest.PageSize, tagFilterRequest.SearchName, tagFilterRequest.SortOption, tagFilterRequest.NewArticleIds);
            var mappedTags = _mapper.Map<List<GetAllResponse>>(pagedTags.Items);

            var pagedResponse = new PagedResult<GetAllResponse>
            {
                Items = mappedTags,
                Page = tagFilterRequest.Page,
                PageSize = tagFilterRequest.PageSize,
                TotalItems = pagedTags.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedTags.TotalItems / (double)tagFilterRequest.PageSize)
            };


            var response = new ResponseDTO<PagedResult<GetAllResponse>>(message: "Tags retrieved successfully", isSuccess: true, data: pagedResponse, errors: null);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTagAsync([FromBody] CreateTagRequest createTagRequest)
        {
            var mappedRequest = _mapper.Map<Tag>(createTagRequest);

            await _tagService.CreateTagAsync(mappedRequest);

            // Implementation for creating a tag would go here
            return StatusCode(201, new ResponseDTO<object>("Tag created successfully", true, null, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagAsync([FromRoute] int id)
        {
            try
            {
                await _tagService.DeleteTagAsync(id);
                return Ok(new ResponseDTO<object>("Tag deleted successfully", true, null, null));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ResponseDTO<object>("Tag not found", false, null, null));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagRequest request)
        {
            

            try
            {
                var entity = _mapper.Map<Tag>(request);

                await _tagService.UpdateTagAsync(id, entity);
                return Ok(new ResponseDTO<object>("Tag updated successfully", true, null, null));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ResponseDTO<object>("Tag not found", false, null, null));
            }
            
        }
    }
}
