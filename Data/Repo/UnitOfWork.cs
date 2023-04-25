using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Interfaces;
namespace WebAPICore.Data.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
          private readonly ApplicationDbContext _Context;

       

        public UnitOfWork(ApplicationDbContext context)
        {
           this._Context = context;
        }
        public IPatientRepositry PatientRepositry => new PatientRepositry(_Context);
        public   IUserRepositry UserRepositry  => new  UserRepositry(_Context);
     public   IRefreshTokenGeneratorRepository RefreshTokenGenerator  => new  RefreshTokenGeneratorRepository(_Context);

        public IRefreshTokenGeneratorRepository RefreshTokenGeneratorRepository => throw new NotImplementedException();

        public async Task<bool> SaveAsync()
        {
           return await _Context.SaveChangesAsync()>0;
        }
    }
}