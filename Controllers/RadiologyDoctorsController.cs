using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBOSWebApplication.Models;

namespace EBOSWebApplication.Controllers
{
    public class RadiologyDoctorsController : Controller
    {
        private readonly Context _context;

        public RadiologyDoctorsController(Context context)
        {
            _context = context;
        }

        // GET: RadiologyDoctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.RadiologyDoctors.ToListAsync());
        }

        // GET: RadiologyDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radiologyDoctor = await _context.RadiologyDoctors
                .FirstOrDefaultAsync(m => m.DoctorNumber == id);
            if (radiologyDoctor == null)
            {
                return NotFound();
            }

            return View(radiologyDoctor);
        }

        // GET: RadiologyDoctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RadiologyDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorNumber,DoctorName,Address,Country,DateofBirth,Medicalspecialty,Phone")] RadiologyDoctor radiologyDoctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(radiologyDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(radiologyDoctor);
        }

        // GET: RadiologyDoctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radiologyDoctor = await _context.RadiologyDoctors.FindAsync(id);
            if (radiologyDoctor == null)
            {
                return NotFound();
            }
            return View(radiologyDoctor);
        }

        // POST: RadiologyDoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorNumber,DoctorName,Address,Country,DateofBirth,Medicalspecialty,Phone")] RadiologyDoctor radiologyDoctor)
        {
            if (id != radiologyDoctor.DoctorNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(radiologyDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RadiologyDoctorExists(radiologyDoctor.DoctorNumber))
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
            return View(radiologyDoctor);
        }

        // GET: RadiologyDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radiologyDoctor = await _context.RadiologyDoctors
                .FirstOrDefaultAsync(m => m.DoctorNumber == id);
            if (radiologyDoctor == null)
            {
                return NotFound();
            }

            return View(radiologyDoctor);
        }

        // POST: RadiologyDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var radiologyDoctor = await _context.RadiologyDoctors.FindAsync(id);
            _context.RadiologyDoctors.Remove(radiologyDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RadiologyDoctorExists(int id)
        {
            return _context.RadiologyDoctors.Any(e => e.DoctorNumber == id);
        }
    }
}
