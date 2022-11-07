using Microsoft.AspNetCore.Mvc;

namespace ExamOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebHelper _webHelper;
        public HomeController()
        {
            _webHelper = new WebHelper();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
