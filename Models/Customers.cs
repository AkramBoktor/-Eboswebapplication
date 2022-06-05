using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models
{
    public class Customers
    {

        public Customers()
        {
            Calls = new List<Calls>();
        }

        [Key]
        public int CustomerNo { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Please Enter Valid Doctor Name"), MinLength(5)]
        [Display(Name = "Customer Name")]

        public string CustomerName { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Please Enter Valid SurName"), MinLength(5)]
        [Display(Name = "Customer Surname")]
        public string Customersurname { get; set; }
        public string Address { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public string Country { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        public virtual ICollection<Calls> Calls { get; set; }

    

    }

}