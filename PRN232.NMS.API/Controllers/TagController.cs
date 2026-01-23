using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.API.Models.RequestModels;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.API.Models.ResponseModels.TagResponses;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.API.Controllers
{
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

        [HttpPost("api/tags/{id}")]
        public async Task<IActionResult> GetTagById([FromRoute] int id)
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
    }
}
