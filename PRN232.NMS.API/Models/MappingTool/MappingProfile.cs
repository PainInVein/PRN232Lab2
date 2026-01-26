using AutoMapper;
using PRN232.NMS.API.Models.RequestModels.CategoryRequests;
using PRN232.NMS.API.Models.RequestModels.NewsArticleRequests;
using PRN232.NMS.API.Models.RequestModels.SystemAccountRequests;
using PRN232.NMS.API.Models.RequestModels.TagRequests;
using PRN232.NMS.API.Models.ResponseModels.NewsArticleResponse;
using PRN232.NMS.API.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.API.Models.ResponseModels.TagResponses;
using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.API.Models.MappingTool
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create your mappings here
            CreateMap<SystemAccount, LoginResponse>();
            CreateMap<SystemAccount, UserResponse>();
            CreateMap<CreateSystemAccountRequest, SystemAccount>();
            CreateMap<UpdateSystemAccountRequest, SystemAccount>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Tag, GetByIdResponse>()
            .ForMember(dest => dest.NewsArticles,
                       opt => opt.MapFrom(src => src.NewsArticles));

            CreateMap<NewsArticle, RelatedNewsArticleResponse>();

            CreateMap<Tag, GetAllResponse>();

            CreateMap<CreateNewsArticleRequest, NewsArticle>();
            CreateMap<UpdateNewsArticleRequest, NewsArticle>();
            CreateMap<NewsArticle, NewsArticleResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.AccountName))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()));

            CreateMap<CreateTagRequest, Tag>();

            CreateMap<UpdateTagRequest, Tag>();

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, PRN232.NMS.API.Models.ResponseModels.CategoryResponses.GetByIdResponse>()
                .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null))
                .ForMember(dest => dest.ChildCategories, opt => opt.MapFrom(src => src.InverseParentCategory));
            CreateMap<Category, PRN232.NMS.API.Models.ResponseModels.CategoryResponses.GetAllResponse>();
            CreateMap<Category, PRN232.NMS.API.Models.ResponseModels.CategoryResponses.CategoryMinimalResponse>();
        }
    }
}
