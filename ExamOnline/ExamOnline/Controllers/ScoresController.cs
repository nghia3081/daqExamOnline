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
        private WebHelper _webHelper;
        public ScoresController(ExamOnlineContext context)
        {
            _context = context;
        }

        // GET: Scores
        public IActionResult Index()
        {
            _webHelper = new WebHelper(HttpContext);
            if (!_webHelper.isLoggedIn()) return RedirectToAction("Index", "AccountsController");
            if (_webHelper.isAdmin()) return IndexAdmin();
            string email = _webHelper.SessionGet("username");
            var examOnlineContext = _context.Scores.Include(s => s.Exam).ThenInclude(e => e.Questions).Include(s => s.Student).Where(s => s.StudentEmail.Equals(email));
            return View( examOnlineContext.ToList());
        }
        public IActionResult IndexAdmin()
        {
            string email = _webHelper.SessionGet("username");
            var subject = _context.Subjects.Include(s => s.Students).Where(s => s.TeacherEmail.Equals(email)).ToList();
            return View(subject);
        }
    }
}
