using AutoMapper;
using GitHub.Searcher.Business.Entities;
using GitHub.Searcher.Business.Entities.GitHubResponse;

namespace GitHub.Searcher.Business;

internal class BusinessMapperProfile : Profile
{
    public BusinessMapperProfile()
    {
        CreateMap<GitHubPullRequestResponse, PullRequestModel>(MemberList.None)
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.UrlToPage, opt => opt.MapFrom(src => src.HtmlUrl))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.User.Login))
            .ForMember(dest => dest.CreatorAvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Body.Substring(0, Math.Min(src.Body.Length, 100))))
            .ForMember(src => src.Number, opt => opt.MapFrom(src => src.Number));
        
        CreateMap<GitHubCommitResponse, CommitModel>(MemberList.None)
            .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Sha))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Commit.Author.Name))
            .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.Commit.Author.Email))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Commit.Author.Date))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Commit.Message));
    }
}