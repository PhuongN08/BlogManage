
using AutoMapper;
using BlogManage.Models;
using AutoMapperProfile = AutoMapper.Profile;
using BlogManage.ViewModel.PublicVM;
using BlogManage.ViewModel.AuthenVM;
using BlogManage.ViewModel.AdminVM;
using BlogManage.ViewModel.WriterVM;
using BlogManage.ViewModel.ManagerVM;
namespace BlogManage.AutoMapper
{
    public class ApplicationMapper : AutoMapperProfile
    {
        public ApplicationMapper()
        {
            //Account
            CreateMap<Account, AccountVM>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Profiles.FirstOrDefault()!.Name))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Profiles.FirstOrDefault()!.Email))
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Profiles.FirstOrDefault()!.Role.RoleName))
                 .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdateDate))
                 .ReverseMap();
            CreateMap<Models.Profile, AccountDetailsVM>()
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.Id))
               .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex ? "Nam" : "Nữ"))
               .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
               .ReverseMap();
            CreateMap<CreateAccountVM, Account>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => true))
                .ReverseMap();
            CreateMap<CreateAccountVM, Models.Profile>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex == Gender.Nam))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(_ => "/profile/default.jpg"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.Now)))
                .ReverseMap();

            //Blog
            CreateMap<Blog, BlogVM>()
                 .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.AuthorDisplayName, otp => otp.MapFrom(src => src.Author.DisplayName))
                 .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.Status.StatusName))
                 .ReverseMap();
            CreateMap<BlogCreateVM, Blog>().ReverseMap();
            CreateMap<BlogUpdateVM, Blog>().ReverseMap();
            CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ReverseMap();
            CreateMap<Comment, CreateAccountVM>().ReverseMap();
            CreateMap<Blog, BlogSumVM>()
               .ForMember(dest => dest.CategoryName, otp => otp.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.AuthorDisplayName, otp => otp.MapFrom(src => src.Author.DisplayName))
                 .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.Status.StatusName))
                 .ReverseMap();

            //Category
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Category, CategoryCreateVM>().ReverseMap();
            //Authen
            CreateMap<RegisterVM, Account>();
            CreateMap<RegisterVM, Models.Profile>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2));

            //public 
            CreateMap<Models.Profile, MyProfileVM>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.Id))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex ? "Nam" : "Nữ"))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();
        }
    }
}
