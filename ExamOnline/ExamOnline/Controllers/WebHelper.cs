using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExamOnline.Controllers
{
    public class WebHelper : Controller
    {
        private readonly ISession _session;
        public WebHelper()
        {
            _session = HttpContext.Session;
        }
        public bool isAdmin()
        {
            if (!isLoggedIn()) return false;
            string role = Encoding.UTF8.GetString(_session.Get("Role"));
            return role.Equals(Common.CommonConfig.ADMIN_ROLE_ID);
        }
        public bool isLoggedIn()
        {
            return _session.Get("username") == null;
        }
        public void SessionAdd(string key, string value)
        {
            if(!string.IsNullOrEmpty(value)) _session.Set(key, Encoding.UTF8.GetBytes(value));
        }
        public string SessionGet(string key)
        {
            var bytes = _session.Get(key);
            if (bytes is null) return string.Empty;
            return Encoding.UTF8.GetString(bytes);
        }
        public void SessionRemove(string key)
        {
            if (_session.TryGetValue(key, out var value)) _session.Remove(key);
        }
    }
}
