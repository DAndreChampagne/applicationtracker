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

namespace ApplicationTracker.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Uri ApiUri = new(@"https://localhost:7281/api/");

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Applications
        public async Task<IActionResult> Index()
        {
            // var applicationDbContext = _context.Applications.Include(a => a.Company).Include(a => a.Contact);
            // return View(await applicationDbContext.ToListAsync());

            using var client = new HttpClient();
            client.BaseAddress = ApiUri;

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(@"applications");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            
            var content = response.Content;
            var stream = await response.Content.ReadAsStreamAsync() ?? Stream.Null;

            var items = await JsonSerializer.DeserializeAsync<List<Application>>(stream);

            return View(items);
        }

        // GET: Admin/Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Company)
                .Include(a => a.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Admin/Applications/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name");
            return View();
        }

        // POST: Admin/Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Link,CompanyId,ContactId,Title,Type,Location,MatchPercent,SalaryMin,SalaryMax,Status,ApplicationStatusReason,DateApplied,FollowUps,Notes")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            return View(application);
        }

        // GET: Admin/Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            return View(application);
        }

        // POST: Admin/Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Link,CompanyId,ContactId,Title,Type,Location,MatchPercent,SalaryMin,SalaryMax,Status,ApplicationStatusReason,DateApplied,FollowUps,Notes")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", application.CompanyId);
            ViewData["ContactId"] = new SelectList(_context.Contacts, "Id", "Name", application.ContactId);
            return View(application);
        }

        // GET: Admin/Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Company)
                .Include(a => a.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Admin/Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
