using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamOnline.Models;
using Newtonsoft.Json.Linq;
using ExamOnline.DTOs;

namespace ExamOnline.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ExamOnlineContext _context;
        private WebHelper _webHelper;

        public SubjectsController(ExamOnlineContext context)
        {
            _context = context;

        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            _webHelper = new WebHelper(HttpContext);
            if (!_webHelper.isLoggedIn()) return RedirectToAction("Index", "AccountsController");
            var examOnlineContext = _context.Subjects.Include(s => s.Teacher);
            return View(await examOnlineContext.ToListAsync());
        }
        public IActionResult Index(string query)
        {
            if (!_webHelper.isLoggedIn()) throw new Exception("Please Login");
            var subject = _context.Subjects.Include(s => s.Teacher).Where(e => e.Name.ToLower().Contains(query.ToLower()) || e.Id.ToLower().Contains(query.ToLower())).ToList();
            return View(subject);
        }
        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }
            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .Include(s => s.Exams)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }
    }
}
