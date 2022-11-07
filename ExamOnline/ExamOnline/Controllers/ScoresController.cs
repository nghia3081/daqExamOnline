using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamOnline.Models;

namespace ExamOnline.Controllers
{
    public class ScoresController : Controller
    {
        private readonly ExamOnlineContext _context;

        public ScoresController(ExamOnlineContext context)
        {
            _context = context;
        }

        // GET: Scores
        public async Task<IActionResult> Index()
        {
            var examOnlineContext = _context.Scores.Include(s => s.Exam).Include(s => s.Student);
            return View(await examOnlineContext.ToListAsync());
        }

        // GET: Scores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Scores == null)
            {
                return NotFound();
            }

            var score = await _context.Scores
                .Include(s => s.Exam)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentEmail == id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

    }
}
