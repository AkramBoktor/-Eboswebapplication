using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models
{
    public class Patient
    {
        
        [Key]
        public int PatientNumber { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Please Enter Valid Customer Name"), MinLength(5)]
        [Display(Name = "Patient Name")]

        public string PatientName { get; set; }
    
        public string Address { get; set; }
      
        public string Country { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        [Required]
        [Display(Name = "Doctor appointment")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Doctorappointment { get; set; }
        [Display(Name = "Doctor Name")]
        public int DoctorNo { get; set; }
        public Doctor Doctor { get; set; }

    }
}
