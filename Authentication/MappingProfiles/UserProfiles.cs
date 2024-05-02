using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;

namespace AuthenticationWebApi.MappingProfiles;
public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<RegisterUserRequest,RegisterUser>().ReverseMap();
        CreateMap<LoginUserRequest, LoginUser>().ReverseMap();
    }
}
