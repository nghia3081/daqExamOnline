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
        private readonly WebHelper _webHelper;

        public ExamsController(ExamOnlineContext context)
        {
            _context = context;
            _webHelper = new WebHelper();
        }

        // GET: Exams
        public async Task<JObject> Index()
        {
            var listExam = await _context.Exams.ToListAsync();
            return ResponseModel.Response<JArray>(0, string.Empty, JArray.FromObject(listExam));
        }
        //[HttpPost, Route("DoExam")]
        //public async Task<JObject> DoExam(Exam exam)
        //{
        //    string email = _webHelper.SessionGet("username");
            
        //}
    }
}
