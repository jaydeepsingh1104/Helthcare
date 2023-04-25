using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Dtos
{
    public class PatientDto
    {
public int PatientID { get; set; }
public string FirstName { get; set; }
public string LastName { get; set; }

public DateTime DateOfBirth { get; set; }
public string Gender { get; set; }
public string Address { get; set; }
public string Phone { get; set; }
public string Email { get; set; }
    }
}