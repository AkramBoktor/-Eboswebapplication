using EBOSWebApplication.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBOSWebApplication.Models.Pagination
{
    public class CustomerPaging
    {
        public List<CustomersViewModel> customersViewModel { get; set; }

        public int CurrentPageIndex { get; set; }

       
        public int PageCount { get; set; }
    }
}
