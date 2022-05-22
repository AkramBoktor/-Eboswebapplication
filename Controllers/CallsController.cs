using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBOSWebApplication.Models;
using EBOSWebApplication.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace EBOSWebApplication.Controllers
{
    [Authorize]

    public class CallsController : Controller
    {
        private readonly Context _context;

        public CallsController(Context context)
        {
            _context = context;
        }

        #region Search index
        public async Task<IActionResult> Index(string SearchString)
        {

            var callVM = GetCalls(SearchString);


            return View(await callVM);
        }
        #endregion

        #region Call Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls.Where(m=>m.Id==id)
                .FirstOrDefaultAsync();
            calls.Customer = _context.Customers.Where(c => c.CustomerNo == calls.CustomerNo).FirstOrDefault();
            if (calls == null)
            {
                return NotFound();
            }

            return View(calls);
        }

        #endregion
        #region Calls Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList().Select(x => new SelectListItem { Text = x.CustomerName +" "+x.Customersurname, Value = x.CustomerNo.ToString() });

            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateofCall,TimeofCall,Subject,Description,CustomerNo")] Calls calls)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    calls.Customer = _context.Customers.Where(c => c.CustomerNo == calls.CustomerNo).FirstOrDefault();
                    _context.Add(calls);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
               catch(Exception msg)
                {
                    ModelState.AddModelError("", msg.Message);
                   
                }
            }
            return View(calls);
        }

        #endregion
        #region Call Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls.FindAsync(id);
            ViewBag.Customers = _context.Customers.ToList().Select(x => new SelectListItem { Text = x.CustomerName + " " + x.Customersurname, Value = x.CustomerNo.ToString() });

            if (calls == null)
            {
                return NotFound();
            }
            return View(calls);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateofCall,TimeofCall,Subject,Description,CustomerNo")] Calls calls)
        {
            if (id != calls.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calls);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallsExists(calls.Id))
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
            return View(calls);
        }
        #endregion

        #region Calls Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calls == null)
            {
                return NotFound();
            }

            return View(calls);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calls = await _context.Calls.FindAsync(id);
            _context.Calls.Remove(calls);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallsExists(int id)
        {
            return _context.Calls.Any(e => e.Id == id);
        }

        #endregion


        public async Task<List<CallViewModel>> GetCalls(string searchstring)
        {
            ViewData["CurrentFilter"] = searchstring;

            if (!String.IsNullOrEmpty(searchstring))
            {
              return await (from call in _context.Calls
                 join customer in _context.Customers
                 on call.CustomerNo equals customer.CustomerNo
                 where call.Customer.CustomerName.Contains(searchstring) ||
                 call.Customer.Customersurname.Contains(searchstring)
                 select new CallViewModel
                 {
                     CustomerNo = call.CustomerNo,
                     Customer = customer.CustomerName + " " + customer.Customersurname,
                     DateofCall = call.DateofCall,
                     Description = call.Description,
                     Id = call.Id,
                     Subject = call.Subject,
                     TimeofCall = call.TimeofCall,
                 }).ToListAsync();
            }

            else
            {
                return await (from call in _context.Calls
                 join customer in _context.Customers
                 on call.CustomerNo equals customer.CustomerNo
               
                 select new CallViewModel
                 {
                     CustomerNo = call.CustomerNo,
                     Customer = customer.CustomerName + " " + customer.Customersurname,
                     DateofCall = call.DateofCall,
                     Description = call.Description,
                     Id = call.Id,
                     Subject = call.Subject,
                     TimeofCall = call.TimeofCall,
                 }).ToListAsync();
            }
            
        }

    }
}
