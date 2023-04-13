using System.ComponentModel.DataAnnotations;
namespace WebAPICore.Model
{
      public class Patient
{
  [Key]
public int PatientID { get; set; }
public string FirstName { get; set; }
public string LastName { get; set; }

public DateTime DateOfBirth { get; set; }
public string Gender { get; set; }
public string Address { get; set; }
public string Phone { get; set; }
public string Email { get; set; }
public string ModifiedBy { get; set; }
public DateTime? CreatedDate { get; set; }
public DateTime? ModifiedDate { get; set; }
public bool IsDeleted { get; set; }
}
   
}