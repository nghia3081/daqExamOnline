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
        private readonly WebHelper _webHelper;

        public SubjectsController(ExamOnlineContext context)
        {
            _context = context;
            _webHelper = new WebHelper();
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            if (!_webHelper.isLoggedIn()) return RedirectToAction("Index", "AccountsController");
            var examOnlineContext = _context.Subjects.Include(s => s.Teacher);
            return View(await examOnlineContext.ToListAsync());
        }
        public JObject Index(string query)
        {
            try
            {
                if (!_webHelper.isLoggedIn()) throw new Exception("Please Login");
                var subject = _context.Subjects.Include(s => s.Teacher).Where(e => e.Name.ToLower().Contains(query.ToLower()) || e.Id.ToLower().Contains(query.ToLower())).ToList();
                return ResponseModel.Response<JArray>(0, string.Empty, JArray.FromObject(subject));
            }
            catch (Exception ex)
            {
                return ResponseModel.Response<string>(99, ex.Message, string.Empty);
            }
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
        [HttpPost]
        public JObject JoinSubject(string subjectId)
        {
            try
            {
                string email = _webHelper.SessionGet("username");
                var subject = _context.Subjects.Find(subjectId);
                var account = _context.Accounts.Find(email);
                if (subject is null) throw new Exception("Not found subject");
                if (account is null) throw new Exception("Please login");
                subject.Students.Add(account);
                account.SubjectsOfStudent.Add(subject);
                _context.Update(account);
                _context.Update(subject);
                _context.SaveChanges();
                return ResponseModel.Response<JObject>(0, string.Empty, JObject.FromObject(subject));
            }
            catch (Exception ex)
            {
                return ResponseModel.Response<string>(99, ex.Message, string.Empty);
            }
        }
    }
}
