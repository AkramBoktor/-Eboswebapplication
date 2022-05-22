using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models.ViewModel
{
    public class CallViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Date of Call")]
        public DateTime DateofCall { get; set; }
        [Display(Name = "Time of Call")]
        public TimeSpan TimeofCall { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [Display(Name ="Cutomer No")]
        public int CustomerNo { get; set; }
        [Display(Name = "Cutomer Name")]
        public string Customer { get; set; }
    }
}
