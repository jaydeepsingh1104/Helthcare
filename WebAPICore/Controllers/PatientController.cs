using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPICore.Interfaces;
using WebAPICore.Model;
using WebAPICore.Dtos;
namespace WebAPICore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController: ControllerBase {
   
        private readonly IUnitOfWork iuow;
          private readonly IMapper mapper;
        public PatientController( IUnitOfWork iuow, IMapper mapper ) 
        {          
            this.iuow = iuow;   
            this.mapper = mapper;           
        }

        [HttpGet("Patients")]
        public async Task< IActionResult> GetPatientlist() {

            var patient = await iuow.PatientRepositry.GetPatientlist();
            var PatientDto = mapper.Map<IEnumerable<PatientDto>>(patient);
            return Ok(PatientDto);
          }
       
        
        [HttpPost("post")]
        public async Task< IActionResult> AddPatient(PatientDto patientDto) {
            var patient = mapper.Map<Patient>(patientDto);
            patient.ModifiedBy = "1";
            patient.CreatedDate = DateTime.Now;
            iuow.PatientRepositry.AddPatient(patient);
            await iuow.SaveAsync();          
            return StatusCode(201);
          }
       
          [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto patientDto)
        {
            if(id != patientDto.PatientID)
                return BadRequest("Update not allowed");

            var patientFromDb = await iuow.PatientRepositry.FindPatient(id);

            if (patientFromDb == null){
                return BadRequest("Update not allowed");
            }
            patientFromDb.ModifiedBy = "1";
            patientFromDb.ModifiedDate = DateTime.Now;                      
            mapper.Map(patientDto, patientFromDb);
            await iuow.SaveAsync();
            return StatusCode(200);
        }
        [HttpPut("updatePatientName/{id}")]
        public async Task<IActionResult> UpdatePatientName(int id, PatientDto patientDto)
        {
            var patientFromDb = await iuow.PatientRepositry.FindPatient(id);
           patientFromDb.ModifiedBy = "1";
            patientFromDb.ModifiedDate = DateTime.Now;
            mapper.Map(patientDto, patientFromDb);
            await iuow.SaveAsync();
            return StatusCode(200);
        }

      [HttpDelete("delete/{PatientID}")]
        public async Task< IActionResult> Delete(int PatientID) {
        iuow.PatientRepositry.DeletePatient(PatientID);     
         await iuow.SaveAsync();      
            return Ok(PatientID);
          }
       

    }
 }
        