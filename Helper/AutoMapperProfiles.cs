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
           CreateMap<User, LoginReqDto>().ReverseMap(); 
            CreateMap<User, LoginResDto>().ReverseMap(); 
             CreateMap<User, RegistrationDto>().ReverseMap(); 
             
        }
        
    }
}