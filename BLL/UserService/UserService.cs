using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repositories.AuthRepository;
using PasswordHasherAuth;

namespace BLL.UserServices;
public class UserService:IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository,
        IJwtProvider jwtProvider, IMapper mapper)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task Register(string userName, string email, string password) 
    {
        try
        {
            var hashpassword = _passwordHasher.Generate(password);
            var userDto = UserDTO.Create(Guid.NewGuid(), userName, hashpassword, email);
            var user = _mapper.Map<UserEntity>(userDto);
            await _userRepository.AddAsync(user);
        } 
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> Login(string email, string password) 
    {
        var user = await _userRepository.GetByEmailAsync(email);
        var mappeduser = _mapper.Map<UserDTO>(user);
        var  result = _passwordHasher.Verify(password,user.PasswordHash);
        if (result == false)
        {
            throw new Exception("Failed to login ");
        }
        var token = _jwtProvider.GenerateToken(mappeduser);
        return token;
    }

}
