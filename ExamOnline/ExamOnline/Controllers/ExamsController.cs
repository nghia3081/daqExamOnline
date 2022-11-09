using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamOnline.Models;
using Newtonsoft.Json.Linq;
using ExamOnline.DTOs;

namespace ExamOnline.Controllers
{
    public class ExamsController : Controller
    {
        private readonly ExamOnlineContext _context;
        private WebHelper _webHelper;

        public ExamsController(ExamOnlineContext context)
        {
            _context = context;
            
        }

        // GET: Exams
        public async Task<IActionResult> Index()
        {_webHelper = new WebHelper(HttpContext);
            if (_webHelper.isLoggedIn()) return RedirectToAction("Index", "LoginController");
            var listExam = await _context.Exams.ToListAsync();
            return View(listExam);
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
        [HttpPost, Route("DoExam")]
        public async Task<IActionResult> DoExam(string examId)
        {
            if(!_webHelper.isLoggedIn()) return RedirectToAction("Index", "LoginController");
            string email = _webHelper.SessionGet("username");
           
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id.Equals(examId));
            ViewData["exam"] = exam;
            return View();
        }
        [HttpPost]
        public async Task<JObject> SubmitExam(JObject data)
        {
            try
            {
                string examId = data["examId"].ToString();
                if (!string.IsNullOrEmpty(examId)) throw new Exception("Can not find this exam");
                var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id.Equals(examId));
                var question = exam.Questions;
                int count = 0;
                var studentAnswer = JArray.FromObject(data["studentAnswer"]);
                foreach (var ans in studentAnswer)
                {
                    string questionId = ans["questionId"].ToString();
                    string answerId = ans["answerId"].ToString();
                    if (question.FirstOrDefault(q => q.Id.Equals(questionId)).RightAnswer.Equals(Guid.Parse(answerId))) count++;
                }
                Score score = new Score();
                score.CorrectTotal = count;
                score.StudentEmail = _webHelper.SessionGet("username");
                score.ExamId = Guid.Parse(examId);
                _context.Scores.Add(score);
                await _context.SaveChangesAsync();
                JObject answer = new JObject()
                {
                    {"correct", count },
                    {"total", question.Count }
                };
                return ResponseModel.Response<JObject>(0, string.Empty, answer);
            }
            catch (Exception ex)
            {
                return ResponseModel.Response<string>(99, ex.Message, string.Empty);
            }
        }
    }
}
