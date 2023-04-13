using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Model;
namespace WebAPICore.Interfaces 

{
    public interface IPatientRepositry
    {
         Task<IEnumerable<Patient>> GetPatientlist();
         void AddPatient(Patient patient);
         void DeletePatient(int PatientID);
         Task<Patient> FindPatient(int PatientID);
         
         Task<bool> SaveAsync();
    }
}