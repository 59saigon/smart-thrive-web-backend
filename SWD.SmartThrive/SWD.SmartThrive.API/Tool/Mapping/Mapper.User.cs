using AutoMapper;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.SearchRequest;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.API.Tool.Mapping
{
    public partial class Mapper : Profile
    {
        public void UserMapping()
        {
            CreateMap<User, UserModel>().ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<UserModel, UserRequest>().ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserModel, UserSearchRequest>().ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
