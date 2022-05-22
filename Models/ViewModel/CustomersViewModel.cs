using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models.ViewModel
{
    public class CustomersViewModel
    {

     
        [Key]
        public int CustomerNo { get; set; }

        [Display(Name = "Customer Name")]

        public string CustomerName { get; set; }
        [Display(Name = "Customer Surname")]
        public string Customersurname { get; set; }
        public string Address { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public string Country { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        public int CallsId{ get; set; }

    

    }

}