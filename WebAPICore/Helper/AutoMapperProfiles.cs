using AutoMapper;
using WebAPICore.Model;
using WebAPICore.Dtos;

namespace WebAPICore.Helper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<Patient, PatientDto>().ReverseMap();  
        }
        
    }
}