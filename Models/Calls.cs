using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models
{
    public class Calls
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Call")]

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofCall { get; set; }

        [Display(Name = "Time of Call")]
        [RegularExpression(@"^(?:[01][0-9]|2[0-3]):[0-0][0-0]:[0-0][0-0]$", ErrorMessage = "Invalid time format and hh:mm:ss values.")]
        public TimeSpan TimeofCall { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }
        [Display(Name ="Customer Name")]
        public int CustomerNo { get; set; }
        public Customers Customer { get; set; }

    

    }
}
