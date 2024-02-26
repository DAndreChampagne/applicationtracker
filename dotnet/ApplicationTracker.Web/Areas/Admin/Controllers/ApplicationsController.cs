using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Common.Contexts;
using ApplicationTracker.Model;
using System.Text.Json;
using PluralizeService.Core;
using System.Collections;
using Newtonsoft.Json.Serialization;
using ApplicationTracker.Common.Services;

namespace ApplicationTracker.Web.Areas.Admin.Controllers {

    [Area("Admin")]
    public class ApplicationsController : Controller {

        private readonly ApiService<Application> _api;
        private readonly ApiService<Contact> _contactsApi;
        private readonly ApiService<Company> _companiesApi;

        public ApplicationsController(ApiService<Application> api, ApiService<Contact> contactsApi, ApiService<Company> companiesApi) {
            _api = api;
            _contactsApi = contactsApi;
            _companiesApi = companiesApi;
        }

        // GET: Admin/Applications
        public async Task<IActionResult> Index() {
            return View(await _api.GetAsync());
        }

        // GET: Admin/Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) {
                return NotFound();
            }

            var application = await _api.GetAsync(id.Value);
            if (application is null) {
                return NotFound();
            }

            return View(application);
        }

        // GET: Admin/Applications/Create
        public async Task<IActionResult> Create()
        {
            // ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            // ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name");

            var c1 = await _companiesApi.GetAsync();
            var c2 = await _contactsApi.GetAsync();

            ViewData["CompanyId"] = new SelectList(await _companiesApi.GetAsync(), "Id", "Name");
            ViewData["ContactId"] = new SelectList(await _contactsApi.GetAsync(), "Id", "Name");
            return View();
        }

        // POST: Admin/Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Link,CompanyId,ContactId,Title,Type,Location,MatchPercent,SalaryMin,SalaryMax,Status,ApplicationStatusReason,DateApplied,FollowUps,Notes")] Application application)
        {
            if (ModelState.IsValid) {
                // _context.Add(application);
                // await _context.SaveChangesAsync();
                var result = await _api.PostAsync(application);
                return RedirectToAction(nameof(Index));
            }

            // ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            // ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            ViewData["CompanyId"] = new SelectList(await _companiesApi.GetAsync(), "Id", "Name");
            ViewData["ContactId"] = new SelectList(await _contactsApi.GetAsync(), "Id", "Name");
            return View(application);
        }

        // GET: Admin/Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var application = await _context.Applications.FindAsync(id);
            var application = await _api.GetAsync(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            // ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            // ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            ViewData["CompanyId"] = new SelectList(await _companiesApi.GetAsync(), "Id", "Name");
            ViewData["ContactId"] = new SelectList(await _contactsApi.GetAsync(), "Id", "Name");
            return View(application);
        }

        // POST: Admin/Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Link,CompanyId,ContactId,Title,Type,Location,MatchPercent,SalaryMin,SalaryMax,Status,ApplicationStatusReason,DateApplied,FollowUps,Notes")] Application application) {

            if (id != application.Id)
                return NotFound();

            if (ModelState.IsValid) {
                try {
                    var item = await _api.PutAsync(id, application);
                    if (item is null)
                        return StatusCode(StatusCodes.Status500InternalServerError);
                } catch (DbUpdateConcurrencyException) {
                    if (!await ApplicationExists(application.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            // ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            ViewData["CompanyId"] = new SelectList(await _companiesApi.GetAsync(), "Id", "Name");
            ViewData["ContactId"] = new SelectList(await _contactsApi.GetAsync(), "Id", "Name");
            return View(application);
        }

        // GET: Admin/Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            // var application = await _context.Applications
            //     .Include(a => a.Company)
            //     .Include(a => a.Contact)
            //     .FirstOrDefaultAsync(m => m.Id == id);
            var application = _api.GetAsync(id.Value);
        
            if (application is null)
                return NotFound();
            return View(application);
        }

        // POST: Admin/Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // var application = await _context.Applications.FindAsync(id);
            var application = await _api.GetAsync(id);
            if (application != null) {
                // _context.Applications.Remove(application);
                await _api.DeleteAsync(id);
            }

            // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ApplicationExists(int id)
        {
            // return _context.Applications.Any(e => e.Id == id);
            return await _api.ExistsAsync(id);
        }
    }
}
