using Microsoft.AspNetCore.Mvc;
using System.Text;
namespace ExamOnline.Controllers
{
    public class WebHelper : Controller
    {
        private readonly ISession _session;
        public WebHelper(HttpContext context)
        {
            _session = context.Session;
        }
        public bool isAdmin()
        {
            if (!isLoggedIn()) return false;
            string role = _session.GetString("role");
            return role.Equals(Common.CommonConfig.ADMIN_ROLE_ID);
        }
        public bool isLoggedIn()
        { 
            return !string.IsNullOrEmpty(_session.GetString("username"));
        }
        public void SessionAdd(string key, string value)
        {
            if(!string.IsNullOrEmpty(value)) _session.SetString(key,value);
        }
        public string SessionGet(string key)
        {
            var value = _session.GetString(key);
           
            return string.IsNullOrEmpty(value)?string.Empty:value;
        }
        public void SessionRemove(string key)
        {
            if (!string.IsNullOrEmpty(SessionGet(key))) _session.Remove(key);
        }
    }
}
