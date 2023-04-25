using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPICore.Dtos;
using WebAPICore.Interfaces ;
using WebAPICore.Model;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace WebAPICore.Data.Repo
{
    public class UserRepositry:IUserRepositry
    {
        private readonly ApplicationDbContext contetx;
       

        public UserRepositry(ApplicationDbContext contetx)
         {
            this.contetx = contetx;
            
         }

        public async Task<User> Authenticate(string userName, string password)
        {
   
            var user =  await contetx.Users.FirstOrDefaultAsync(x=>x.Username==userName);
             if (user == null || user.PasswordKey == null)
                return null;
                
                  if (!MatchPasswordHash(password, user.Password, user.PasswordKey))
                return null;

            return user;
        }
        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i=0; i<passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
         
     
         }
        }
        public void Register(RegistrationDto loginReqDto)
        {            
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginReqDto.Password));

            }
 
             User user = new User();
            user.Username = loginReqDto.Username;
            user.email=loginReqDto.email;
            user.gender=loginReqDto.gender;
            user.role=loginReqDto.role;           
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;
            
             user.ModifiedBy = "1";
            user.CreatedDate = DateTime.Now;
             this.contetx.Users.Add(user);
        }
         public async Task<bool> UserAlreadyExists(string userName)
        {
            return await contetx.Users.AnyAsync(x => x.Username == userName);
        }
        public async Task<IEnumerable<User>> GetUserslist()
        {
          return await contetx.Users.ToListAsync();
        }
           public async  Task<User> FindUser(string userName)
        {
            return await contetx.Users.FirstOrDefaultAsync(x => x.Username == userName);
        }

        public void DeleteUser(string userName)
        {
           var user = contetx.Users.FirstOrDefault(x => x.Username == userName);
            contetx.Users.Remove(user);
        }
     
    }
}