using AutoMapper;
using PRN232.NMS.API.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.API.Models.MappingTool
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create your mappings here
            CreateMap<SystemAccount, LoginResponse>();
        }
    }
}
