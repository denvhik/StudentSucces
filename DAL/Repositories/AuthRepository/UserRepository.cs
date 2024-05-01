using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.AuthRepository;

public class UserRepository: IUserRepository
{
    private readonly StudentSuccesContext _studentSuccesContext;
    public UserRepository(StudentSuccesContext studentSuccesContext) 
    {
        _studentSuccesContext = studentSuccesContext;
    }
    public  async Task AddAsync(UserEntity user)
    {
        try
        {
            await _studentSuccesContext.Users.AddAsync(user);
            await _studentSuccesContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<UserEntity> GetByEmailAsync(string email)
    {
       var entity = await _studentSuccesContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();
        return entity;
    }
}
