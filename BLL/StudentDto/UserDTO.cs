namespace BLL.StudentDto;
public class UserDTO
{
        private UserDTO(Guid id, string userName, string passwordHash, string email)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public static UserDTO Create(Guid id, string userName, string passworgHash, string email)
        {
            return new UserDTO(id, userName, passworgHash, email);
        }
    }
