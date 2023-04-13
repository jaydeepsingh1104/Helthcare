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

        public async Task<bool> SaveAsync()
        {
           return await _Context.SaveChangesAsync()>0;
        }
    }
}