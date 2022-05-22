using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBOSWebApplication.Models;
using EBOSWebApplication.Models.ViewModel;
using EBOSWebApplication.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using FastReport.Web;
using FastReport.Data;
using System.IO;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using EBOSWebApplication.reports;

namespace EBOSWebApplication.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly Context _context;
        IHostEnvironment _hostEnvironment;
        IConfiguration _configuration;
        List<ReportViewModel> reportList = new List<ReportViewModel>();
        public CustomersController(Context context, IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;

        }


        public async Task<IActionResult> Index()
        {


            return View(await this.GetCustomers(1));
        }

        #region Paging action
        [HttpPost]
        public async Task<IActionResult> Index(int currentPageIndex)
        {
            return View(await this.GetCustomers(currentPageIndex));
        }

        public async Task<CustomerPaging> GetCustomers(int currentPage)
        {
            CustomerPaging customerModel = new CustomerPaging();
            var customerListCount = GetCustomersRowsCount();
            int maxRows = 10;
            customerModel.customersViewModel = GetCustomerWithPaging(maxRows, currentPage);

            double pageCount = (double)((decimal)customerListCount / Convert.ToDecimal(maxRows));
            customerModel.PageCount = (int)Math.Ceiling(pageCount);

            customerModel.CurrentPageIndex = currentPage;

            return await Task.FromResult<CustomerPaging>(customerModel);
        }

        #endregion

        #region Customer Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerNo == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        #endregion

        #region Customer Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerNo,CustomerName,Customersurname,Address,PostCode,Country,DateofBirth")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        #endregion

        #region Customer Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerNo,CustomerName,Customersurname,Address,PostCode,Country,DateofBirth")] Customers customers)
        {
            if (id != customers.CustomerNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.CustomerNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        #endregion

        #region Customer Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerNo == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerNo == id);
        }

        #endregion

        #region Pagination
        //Get Customers Row Count
        public int GetCustomersRowsCount()
        {
            return (from call in _context.Calls
                    join customer in _context.Customers
                    on call.CustomerNo equals customer.CustomerNo
                    select customer).Count();
        }

        //Get List of Customers with Paging
        private List<CustomersViewModel> GetCustomerWithPaging(int maxRows, int currentPage)
        {
            return (from call in _context.Calls
                    join customer in _context.Customers
                    on call.CustomerNo equals customer.CustomerNo
                    select new CustomersViewModel
                    {
                        CustomerNo = customer.CustomerNo,
                        Address = customer.Address,
                        Country = customer.Country,
                        CustomerName = customer.CustomerName,
                        Customersurname = customer.Customersurname,
                        DateofBirth = customer.DateofBirth,
                        PostCode = customer.PostCode,
                        CallsId = call.Id
                    }).OrderBy(customer => customer.CustomerNo)
                           .Skip((currentPage - 1) * maxRows)
                           .Take(maxRows).ToList();
        }

        #endregion


        #region Report Customer Call
        public IActionResult Report()
        {
            var webReport = new WebReport();
            var reportList = GetDataByCommandQuery(webReport);
            webReport.Report.RegisterData(reportList, "Customers");
            return View(webReport);

        }

        public List<ReportViewModel> GetDataByCommandQuery(WebReport webReport)
        {
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "Report_EachCustomers_WithCalls.frx"));
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                string query = @"select [CustomerName] as First_Name,[Customersurname] as Second_Name,[Address],[Country] ,
    [dbo].[Calls].Description as Description_call , [dbo].[Calls].Subject as Subject_Call
    from [dbo].[Customers]
    join [dbo].[Calls]
    on [dbo].[Calls].CustomerNo = [dbo].[Customers].CustomerNo

    Order by [dbo].[Customers].CustomerNo;
                                         ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                var dr = cmd.ExecuteReader();

                try
                {

                    while (dr.Read())
                    {
                        ReportViewModel report = new ReportViewModel();
                        report.First_Name = dr.GetString(0);
                        report.Second_Name = dr.GetString(1);
                        report.Address = dr.GetString(2);
                        report.Country = dr.GetString(3);
                        report.Description_call = dr.GetString(4);
                        report.Subject_Call = dr.GetString(5);

                        //display retrieved record

                        reportList.Add(report);
                    }
                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    var x = reportList;
                    cmd.Connection.Close();
                    dr.Close();

                }


            }

            return reportList;

        }
    }
}
        
        #endregion


   
