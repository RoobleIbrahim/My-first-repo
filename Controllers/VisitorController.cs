using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Visitors.Data;
using Visitors.Models;

namespace Visitors.Controllers
{
    [Authorize]
    public class VisitorController : Controller
    {

        private readonly ApplicationDbContext _context;

        public VisitorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Visitor
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visitors.Include(v => v.Category).Include(v => v.Place).Include(v => v.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Title");
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceName");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "FulllName");
            return View();
        }

        // POST: Admin/Visitor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitorId,UserID,PlaceId,CategoryId,Description,VisitedDate,Status")] Visitor visitor)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {


                visitor.VisitedDate = DateTime.Now;
                visitor.UserID = userId;
                _context.Add(visitor);
                await _context.SaveChangesAsync();

                //Activity Logs
                var activity = new ActivityLog
                {
                    Action = "Create",
                    Timestamp = DateTime.Now,
                    BrowserInfo = Request.Headers["User-Agent"].ToString(),
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserID = userId
                };
                _context.Add(activity);
                await _context.SaveChangesAsync();

                TempData["MESSAGE"] = "visitor successfully created";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Error Logs
                var logs = new ErrorLog
                {
                    UserID = userId,
                    ErrorDescription = ex.Message,
                    LineNumber = 1,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Traceback = ex.InnerException.ToString(),
                    Timestamp = DateTime.Now,
                    BrowserInfo = Request.Headers["User-Agent"].ToString(),
                };
                _context.Add(logs);
                await _context.SaveChangesAsync();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", visitor.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId", visitor.PlaceId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "FulllName", visitor.UserID);
            return View(visitor);
        }

        // GET: Admin/Visitor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Title", visitor.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceName", visitor.PlaceId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "FulllName", visitor.UserID);
            return View(visitor);
        }

        // POST: Admin/Visitor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitorId,UserID,PlaceId,CategoryId,Description,VisitedDate,Status")] Visitor visitor)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != visitor.VisitorId)
            {
                return NotFound();
            }

            try
            {
                visitor.VisitedDate = DateTime.Now;
                visitor.UserID = userId;
                _context.Update(visitor);
                await _context.SaveChangesAsync();

                TempData["MESSAGE"] = "Visitor successfully Updated";

                //Activity Logs
                var activity = new ActivityLog
                {
                    Action = "Update",
                    Timestamp = DateTime.Now,
                    BrowserInfo = Request.Headers["User-Agent"].ToString(),
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserID = userId
                };
                _context.Add(activity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                //Error Logs
                var logs = new ErrorLog
                {
                    UserID = userId,
                    ErrorDescription = ex.Message,
                    LineNumber = 1,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Traceback = ex.InnerException.ToString(),
                    Timestamp = DateTime.Now,
                    BrowserInfo = Request.Headers["User-Agent"].ToString(),
                };
                _context.Add(logs);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", visitor.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "PlaceId", visitor.PlaceId);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "FulllName", visitor.UserID);
            return View(visitor);
        }

        // GET: Admin/Visitor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors
                .Include(v => v.Category)
                .Include(v => v.Place)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VisitorId == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // POST: Admin/Visitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor != null)
            {



                _context.Visitors.Remove(visitor);
                await _context.SaveChangesAsync();

                TempData["MESSAGE"] = "Visitor successfully Deleted";
            }

            //Activity Logs
            var activity = new ActivityLog
            {
                Action = "Delete",
                Timestamp = DateTime.Now,
                BrowserInfo = Request.Headers["User-Agent"].ToString(),
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserID = userId
            };
            _context.Add(activity);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitors.Any(e => e.VisitorId == id);
        }
    }

}