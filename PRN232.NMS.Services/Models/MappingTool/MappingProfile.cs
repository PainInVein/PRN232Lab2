using AutoMapper;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.SystemAccountModels;
using PRN232.NMS.Services.BusinessModel.TagModels;
using PRN232.NMS.Services.Models.RequestModels.Auth;
using PRN232.NMS.Services.Models.RequestModels.CategoryRequests;
using PRN232.NMS.Services.Models.RequestModels.NewsArticleRequests;
using PRN232.NMS.Services.Models.RequestModels.SystemAccountRequests;
using PRN232.NMS.Services.Models.RequestModels.TagRequests;
using PRN232.NMS.Services.Models.ResponseModels.CategoryResponses;
using PRN232.NMS.Services.Models.ResponseModels.NewsArticleResponse;
using PRN232.NMS.Services.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.Services.Models.ResponseModels.TagResponses;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.TagModels;
using PRN232.NMS.Services.BusinessModel.NewsArticleModels;

namespace PRN232.NMS.Services.Models.MappingTool
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Request/ResponseDTO mappings
            CreateMap<SystemAccount, LoginResponse>();
            CreateMap<SystemAccount, UserResponse>();
            CreateMap<SystemAccountBusinessModel, UserResponse>();
            CreateMap<CreateSystemAccountRequest, SystemAccount>();
            CreateMap<UpdateSystemAccountRequest, SystemAccount>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TagWithNewsArticle, GetTagByIdResponse>()
            .ForMember(dest => dest.NewsArticles,
                       opt => opt.MapFrom(src => src.NewsArticles));

            CreateMap<RelatedNewsArticleBusinessModel, RelatedNewsArticleResponse>();

            CreateMap<Tag, GetAllTagResponse>();

            CreateMap<CreateNewsArticleRequest, NewsArticle>();
            CreateMap<UpdateNewsArticleRequest, NewsArticle>();
            CreateMap<NewsArticle, NewsArticleBusinessModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.AccountName))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()));

            CreateMap<CreateTagRequest, Tag>();

            CreateMap<UpdateTagRequest, Tag>();

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, ResponseModels.CategoryResponses.GetByIdResponse>()
                .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null))
                .ForMember(dest => dest.ChildCategories, opt => opt.MapFrom(src => src.InverseParentCategory));
            CreateMap<Category, ResponseModels.CategoryResponses.GetAllResponse>();
            CreateMap<Category, CategoryMinimalResponse>();

            CreateMap<SystemAccount, LoginRequestModel>().ReverseMap();
            CreateMap<SystemAccount, RegisterRequestModel>().ReverseMap();


            // BusinessModels
            CreateMap<NewsArticle, RelatedNewsArticleBusinessModel>();

            CreateMap<Tag, TagWithNewsArticle>()
                .ForMember(dest => dest.NewsArticles,
                           opt => opt.MapFrom(src => src.NewsArticles));

            CreateMap<NewsArticleBusinessModel, NewsArticleResponse>();
            CreateMap<SystemAccount, SystemAccountBusinessModel>();
        }
    }
}
