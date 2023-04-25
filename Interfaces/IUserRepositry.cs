using WebAPICore.Dtos;
using WebAPICore.Model;

namespace WebAPICore.Interfaces
{
    public interface IUserRepositry
    {
          Task<User> Authenticate(string userName, string password);  
         void Register(RegistrationDto LoginReqDto); 
         Task<bool> UserAlreadyExists(string userName);
          Task<IEnumerable<User>> GetUserslist();
           Task<User> FindUser(string userName);
                void DeleteUser(string userName);
    }
}