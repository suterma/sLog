using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sLog.Models;

namespace sLog.Controllers
{
    public class LogsController : Controller
    {
        private readonly sLogContext _context;

        public LogsController(sLogContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var sLogContext = _context.Log.Include(l => l.Registration);
            return View(await sLogContext.ToListAsync());
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log
                .Include(l => l.Registration)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            ViewData["RegistrationId"] = new SelectList(_context.Registration, "RegistrationId", "RegistrationId");
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogId,Timestamp,Data,MimeType,RegistrationId")] Log log)
        {
            if (ModelState.IsValid)
            {
                log.LogId = Guid.NewGuid();
                _context.Add(log);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegistrationId"] = new SelectList(_context.Registration, "RegistrationId", "RegistrationId", log.RegistrationId);
            return View(log);
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            ViewData["RegistrationId"] = new SelectList(_context.Registration, "RegistrationId", "RegistrationId", log.RegistrationId);
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LogId,Timestamp,Data,MimeType,RegistrationId")] Log log)
        {
            if (id != log.LogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogExists(log.LogId))
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
            ViewData["RegistrationId"] = new SelectList(_context.Registration, "RegistrationId", "RegistrationId", log.RegistrationId);
            return View(log);
        }

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log
                .Include(l => l.Registration)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var log = await _context.Log.FindAsync(id);
            _context.Log.Remove(log);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogExists(Guid id)
        {
            return _context.Log.Any(e => e.LogId == id);
        }
    }
}
