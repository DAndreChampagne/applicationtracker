using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Common.Contexts;
using ApplicationTracker.Model;
using ApplicationTracker.Common.Services;

namespace ApplicationTracker.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompaniesController : Controller
    {
        private readonly ApiService<Company> _api;

        public CompaniesController(ApiService<Company> api) {
            _api = api;   
        }

        // GET: Admin/Companies
        public async Task<IActionResult> Index()
        {
            return View(await _api.GetAsync());
        }

        // GET: Admin/Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var item = await _api.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Admin/Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CompanyId")] Company item)
        {
            if (ModelState.IsValid)
            {
                await _api.PostAsync(item);
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: Admin/Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _api.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Admin/Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CompanyId")] Company item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _api.PutAsync(id, item);
                    if (result is null)
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CompanyExistsAsync(item.Id))
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
            return View(item);
        }

        // GET: Admin/Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _api.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Admin/Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _api.GetAsync(id);
            if (item != null)
            {
                await _api.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyExistsAsync(int id)
        {
            return await _api.ExistsAsync(id);
        }
    }
}
