using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Interfaces
{
    public interface IRefreshTokenGeneratorRepository
    {
        string GenerateToken(string username);
    }
}