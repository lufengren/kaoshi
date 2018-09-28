using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using kaoshi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace kaoshi.Controllers
{
    public class HomeController : Controller
    {
        private kaoshiContext _context;
        public HomeController(kaoshiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(users newuser, string confirmpass)
        {
            if (newuser.password == confirmpass)
            {
                if (ModelState.IsValid)
                {
                    users result = _context.users.SingleOrDefault(users => users.email == newuser.email);
                    if (result == null)
                    {
                        PasswordHasher<users> Hasher = new PasswordHasher<users>();
                        newuser.password = Hasher.HashPassword(newuser, newuser.password);
                        _context.Add(newuser);
                        _context.SaveChanges();
                        //HttpContext.Session.SetInt32("userid", result.UserId);
                        HttpContext.Session.SetString("username", newuser.firstname);
                        ViewBag.msg = "Please login";
                        return View();
                    }
                    else
                    {
                        ViewBag.err = "Email already exists";
                        return View(newuser);
                    }
                }
                else
                {
                    return View(newuser);
                }
            }
            else
            {
                ViewBag.err = "Confirm password did not match";
                return View(newuser);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login(string email, string password)
        {
            users result = _context.users.SingleOrDefault(users => users.email == email);
            if (result != null)
            {
                var Hasher = new PasswordHasher<users>();
                if (0 != Hasher.VerifyHashedPassword(result, result.password, password))
                {
                    HttpContext.Session.SetInt32("userid", result.UserId);
                    HttpContext.Session.SetString("username", result.firstname);

                    return RedirectToAction("Home");
                }
                else
                {
                    ViewBag.mssg = "Wrong password";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.mssg = "Email doesn't exist";
                return View("Index");
            }
        }


        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                List<events> allevents = _context.events
                .Include(x => x.user)
                .Include(y => y.joins)
                .ThenInclude(z => z.user)
                //.OrderByDescending(a =>a.date)
                .Where(b => b.date > DateTime.Now)
                .ToList();
                ViewBag.username = HttpContext.Session.GetString("username");
                ViewBag.userid = HttpContext.Session.GetInt32("userid");
                ViewBag.allevents = allevents;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        [Route("New")]
        public IActionResult New(events newevent, string type)
        {
            if (ModelState.IsValid)
            {
                if (newevent.date > DateTime.Now)
                {
                    events addevent = new events
                    {
                        title = newevent.title,
                        time = newevent.time,
                        date = newevent.date,
                        duration = newevent.duration + type,
                        description = newevent.description,
                        UserId = HttpContext.Session.GetInt32("userid").Value
                    };
                    _context.Add(addevent);
                    _context.SaveChanges();
                    return RedirectToAction("Home");
                }
                else
                {
                    ViewBag.err = "Please input future time";
                    return View(newevent);
                }
            }
            else
            {
                return View(newevent);
            }
        }

        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult activity(int id)
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                events oneevent = _context.events
                .Include(p => p.user)
                .Include(x => x.joins)
                .ThenInclude(y => y.user)
                .SingleOrDefault(events => events.eventid == id);
                ViewBag.userid = HttpContext.Session.GetInt32("userid");
                ViewBag.oneevent = oneevent;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult delete(int id)
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                int userid = HttpContext.Session.GetInt32("userid").Value;
                events RetrievedUser = _context.events.SingleOrDefault(w => w.eventid == id && w.UserId == userid);
                _context.events.Remove(RetrievedUser);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            else
            {
                return View("Index");
            }

        }

        [HttpGet]
        [Route("attend/{id}")]
        public IActionResult attend(int id)
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                joins newevent = new joins
                {
                    UserId = HttpContext.Session.GetInt32("userid").Value,
                    eventid = id
                };
                _context.Add(newevent);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("cancel/{id}")]
        public IActionResult cancel(int id)
        {
            if (HttpContext.Session.GetInt32("userid").HasValue)
            {
                int userid = HttpContext.Session.GetInt32("userid").Value;
                joins RetrievedUser = _context.joins.SingleOrDefault(w => w.UserId == userid && w.eventid == id);
                _context.joins.Remove(RetrievedUser);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("logoff")]
        public IActionResult logoff()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
