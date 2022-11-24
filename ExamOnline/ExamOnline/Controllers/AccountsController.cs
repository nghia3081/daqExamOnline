using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamOnline.Models;
using System.Text;
using Newtonsoft.Json.Linq;
using ExamOnline.DTOs;

namespace ExamOnline.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ExamOnlineContext _context;
        private WebHelper _webHelper;

        public AccountsController(ExamOnlineContext context)
        {
            _context = context;

        }
        [HttpGet]
        public IActionResult Index()
        {
            _webHelper = new WebHelper(HttpContext);
            if (_webHelper.isLoggedIn()) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Login(AccountDTO account)
        {
            _webHelper = new WebHelper(HttpContext);
            JObject response = new JObject();
            try
            {
                if (!AccountExists(account.Email)) throw new Exception("Not found this user");
                var acc = _context.Accounts.Find(account.Email);
                if (checkPassword(account.Password, acc.Password))
                {
                    _webHelper.SessionAdd("username", acc.Email);
                    _webHelper.SessionAdd("role", acc.RoleId.ToString());

                }
                else throw new Exception("Invalid username or password");
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                return View("Index");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            _webHelper = new WebHelper(HttpContext);
            _webHelper.SessionRemove("username");
            return RedirectToAction("Index", "Home");
        }
        private bool checkPassword(string password, string dbPassword)
        {
            _webHelper = new WebHelper(HttpContext);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password)).Equals(dbPassword);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JObject> Create(AccountDTO account)
        {
            _webHelper = new WebHelper(HttpContext);
            try
            {
                if (!_webHelper.isLoggedIn()) throw new Exception("Please reload page and log in");
                if (!_webHelper.isAdmin()) throw new Exception("You do not have this permission");
                Account acc = new Account();
                acc.Email = account.Email;
                byte[] bytes = Encoding.UTF8.GetBytes(account.Password);
                acc.Password = Convert.ToBase64String(bytes);
                acc.RoleId = Guid.Parse(Common.CommonConfig.STUDENT_ROLE_ID);
                acc = _context.Add(acc).Entity;
                await _context.SaveChangesAsync();
                JObject data = JObject.FromObject(acc);
                return ResponseModel.Response<JObject>(0, string.Empty, data);
            }
            catch (Exception ex)
            {
                return ResponseModel.Response<JObject>(99, ex.Message, null);
            }

        }
        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JObject> DeleteConfirmed(string email)
        {
            _webHelper = new WebHelper(HttpContext);
            try
            {
                if (!_webHelper.isLoggedIn()) throw new Exception("Please reload page and log in");
                if (!_webHelper.isAdmin()) throw new Exception("You do not have this permission");
                if (!AccountExists(email)) throw new Exception("Not found account");
                var account = await _context.Accounts.FindAsync(email);
                if (account != null) _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return ResponseModel.Response<string>(0, string.Empty, email);
            }
            catch (Exception ex)
            {
                return ResponseModel.Response<string>(99, ex.Message, string.Empty);
            }

        }
        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.Email == id);
        }
    }
}
