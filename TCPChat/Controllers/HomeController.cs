using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCPChat.Models;

namespace TCPChat.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindByEmail("tcpchatadm@gmail.com");
            if (user.Id == User.Identity.GetUserId()) ViewBag.Role = "adm"; else ViewBag.Role = "usr";
            return View(db.ChatLog);
        }
        
        [HttpPost]
        public ActionResult ChatLog(string nick, string userId, string action)
        {
            var allmessages = db.ChatLog.ToList();
            if (action == "search")
            {
                if (nick!=null && nick != "")
                {
                    allmessages = allmessages.Where(m => m.Nick.Contains(nick)).ToList();
                }
                if(userId!=null && userId != "")
                {

                    allmessages = allmessages.Where(m => m.UserId.Contains(userId)).ToList();
                }
            }
            return PartialView(allmessages);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult ChatRoom()
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindByEmail(User.Identity.Name);
            ViewBag.Ident = User.Identity.GetUserId();
            return View();
        }
    }
}