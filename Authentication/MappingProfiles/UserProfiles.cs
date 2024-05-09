using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;
using Dal.Auth.Model;

namespace AuthenticationWebApi.MappingProfiles;
public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<RegisterUserRequest,RegisterUser>().ReverseMap();
        CreateMap<LoginUserRequest, LoginUser>().ReverseMap();
        CreateMap<User, LoginUser>().ReverseMap();
    }
}
