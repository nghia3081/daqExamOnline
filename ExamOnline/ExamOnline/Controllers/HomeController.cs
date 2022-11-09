using ExamOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExamOnlineContext _context;
        private WebHelper _webHelper;
        public HomeController()
        {
            _context = new ExamOnlineContext();
        }
        public IActionResult Index()
        {
            _webHelper = new WebHelper(HttpContext);
            if (!_webHelper.isLoggedIn()) return RedirectToAction("Index", "Accounts");
            if (_webHelper.isAdmin())
            {
                return IndexAdmin();
            }
            string email = _webHelper.SessionGet("username");
            var subjectOfStudent = _context.Accounts.FirstOrDefault(a => a.Email.Equals(email)).SubjectsOfStudent.ToList();
            ViewData["SubjectOfStudent"] = subjectOfStudent;
            return View();
        }
        public IActionResult IndexAdmin()
        {
            return View();
        }
    }
}
