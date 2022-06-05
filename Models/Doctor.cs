using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models
{
    public class Doctor
    {

        public Doctor()
        {
            Patients = new List<Patient>();
        }

        [Key]
        public int DoctorNumber { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Please Enter Valid Customer Name"), MinLength(5)]
        [Display(Name = "Doctor Name")]

        public string DoctorName { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        [Display(Name = "Medical specialty")]

        public string Medicalspecialty { get; set; }

        public string Phone { get; set; }

      
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
