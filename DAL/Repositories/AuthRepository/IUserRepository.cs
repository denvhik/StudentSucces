using DAL.Models;

namespace DAL.Repositories.AuthRepository;

public interface IUserRepository
{
  
        public Task AddAsync(UserEntity user);
        public Task<UserEntity> GetByEmailAsync(string email);
    
}
