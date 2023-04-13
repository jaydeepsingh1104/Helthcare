using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPICore.Model;
using WebAPICore.Interfaces ;

namespace WebAPICore.Data.Repo
{
    public class PatientRepositry : IPatientRepositry
    {  private readonly ApplicationDbContext _context;
    public PatientRepositry(ApplicationDbContext context)
    {
       this._context = context;  
    }
        public void AddPatient(Patient patient)
        {
           _context.Patients.AddAsync(patient);
        _context.SaveChangesAsync();  
        }

        public void DeletePatient(int PatientID)
        {
           var patient=_context.Patients.Find(PatientID);
            _context.Patients.Remove(patient);
        }

        public Task<Patient> FindPatient(int PatientID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Patient>> GetPatientlist()
        {
          return await _context.Patients.ToListAsync();
        }
         public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync()>0;
           
        }      

        
    }
}