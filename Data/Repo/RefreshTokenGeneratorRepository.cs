using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebAPICore.Interfaces;
using WebAPICore.Model;

namespace WebAPICore.Data.Repo
{
    public class RefreshTokenGeneratorRepository : IRefreshTokenGeneratorRepository
    {
          private readonly ApplicationDbContext context;
       

        public RefreshTokenGeneratorRepository(ApplicationDbContext context)
         {
            this.context = context;
            
         }

        public string GenerateToken(string username)
        {
             var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshToken = Convert.ToBase64String(randomnumber);

                var _user = context.RefreshtokenTables.FirstOrDefault(o => o.UserId == username);
                if (_user != null)
                {
                    _user.RefreshToken = RefreshToken;
                    context.SaveChanges();
                }
                else
                {
                    RefreshtokenTable RefreshtokenTable = new RefreshtokenTable()
                    {
                        UserId=username,
                        TokenId=new Random().Next().ToString(),
                        RefreshToken= RefreshToken,
                        IsActive=true
                    };
                    context.RefreshtokenTables.Add(RefreshtokenTable);
                     context.SaveChanges();
                }

                return RefreshToken;
            }
        }
        // public string GenerateToken(string username)
        // {
        //              var tokenBytes = RandomNumberGenerator.GetBytes(64);
        //     var refreshToken = Convert.ToBase64String(tokenBytes);
        //   //  var tokenInUser = contetx.Users.Any(a => a.RefreshToken == refreshToken);
        //     // if (tokenInUser)
        //     // {
        //     //     return CreateRefreshToken();
        //     // }
        //     // else
        //     // {
        //     //     return refreshToken;
        //     // }
        // }
    }
}