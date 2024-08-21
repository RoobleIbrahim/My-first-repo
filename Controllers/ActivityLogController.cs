using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Visitors.Data;
using Visitors.Enums;
using Visitors.Models;

namespace Visitors.Controllers
{
    [Authorize]
    public class ActivityLogController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public ActivityLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ActivityLog
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActivityLogs.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ActivityLog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.LogID == id);
            if (activityLog == null)
            {
                return NotFound();
            }

            return View(activityLog);
        }
    }
}
