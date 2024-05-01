using BLL.StudentDto;

public interface IJwtProvider
{
    public string GenerateToken(UserDTO user);
}
