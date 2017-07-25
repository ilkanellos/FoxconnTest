using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoxconnTest.Models;

namespace FoxconnTest.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly FoxconntestContext _context;

        public EmployeesController(FoxconntestContext context)
        {
            _context = context;    
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            //Default index page view. Draws data from the SQLite database and lists them
            //as html table objects.
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            //Check to see if the ID parameter exists in the http route.
            //If it doesn't exist a 404 error page is shown.
            if (id == null)
            {
                return NotFound();
            }

            //Since the employeeID is unique for each DB record, using the SingleOrDefault method will either
            //return the employee whose ID we asked for, or if the requested ID does not exist,
            //a 404 error page is returned.
            var employees = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employees/Create
        // Action to open the Create View
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // Action that POSTS the form's data to the SQLite database.
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,EmployeeSurname,MobileNumber,EmailAddress")] Employees employees)
        {
            //Validating that the model is correct and ready to be updated with the new record.
            if (ModelState.IsValid)
            {
                //Adding the new employee to the database
                _context.Add(employees);
                await _context.SaveChangesAsync();

                //If all is done correctly, redirects the user to the Index View.
                return RedirectToAction("Index");
            }
            return View(employees);
        }

        // GET: Employees/Edit/5
        // Action that allows the editing of the selected employee's data.
        public async Task<IActionResult> Edit(long? id)
        {
            // If the ID parameter is absent from the http route, a 404 error page is returned.
            if (id == null)
            {
                return NotFound();
            }

            // At first the selected employee's data are retrieved from the Database and fill in the View's form.
            var employees = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);

            //If the requested employee's ID does not exist, a 404 error page is returned.
            if (employees == null)
            {
                return NotFound();
            }

            // Otherwise the view with the employee's full data is returned.
            return View(employees);
        }

        // POST: Employees/Edit/5
        // Action to "Commit" the edits we made on the selected employee object and save it in the SQLite Database.
        // This action is triggered by the push of the "Save" button.
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EmployeeId,EmployeeName,EmployeeSurname,MobileNumber,EmailAddress")] Employees employees)
        {
            // Querying againast the database to check that the employee's ID exists. If it doesn't exist
            // a 404 error page is returned.
            if (id != employees.EmployeeId)
            {
                return NotFound();
            }

            // Checking to see if the model state is valid and ready to be updated with the new data.
            if (ModelState.IsValid)
            {
                // Try-catch block to perform the update on the SQLite database file.
                // If no problem is found, the new data are saved, replacing the employee's old data.
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }

                // Measures have to be taken to protect against concurrency. If another user is trying to access or edit
                // the specific employee's data, we have to make sure that only one has access to the data and the employee's model.
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(employees);
        }

        // GET: Employees/Delete/5
        // Action to show the Delete View page. Takes the employee's id parameter as an input.
        public async Task<IActionResult> Delete(long? id)
        {
            // If the id parameter is absent from the http route, a 404 error page is returned.
            if (id == null)
            {
                return NotFound();
            }

            // Querying against the database to return the employee object that has the specified employee id.
            var employees = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);

            // If the requested employee's ID does not exist in the database, a 404 error page is returned.
            if (employees == null)
            {
                return NotFound();
            }

            // If everything is correct, the Delete View initializes with the requested employee's data.
            return View(employees);
        }

        // POST: Employees/Delete/5
        // Action to commit the deletion of the specified employee from the database. Takes the employee's id parameter as an input.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employees = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Method to check if the employee's id exists in the database.
        private bool EmployeesExists(long id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
