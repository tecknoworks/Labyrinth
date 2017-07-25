using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using Labyrinth.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace Labyrinth.Controllers
{

    
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Game");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Mail()
        {

            return View("Mail");
        }
        [HttpPost]
        public ActionResult sendMail(string message)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = "smtp.gmail.com";
            client.Timeout = 10000;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("buduxrock4lifex@gmail.com", "96maidoi");
            objeto_mail.From = new MailAddress("budu@gmail.com");
            foreach (var user in db.Users) {
                objeto_mail.To.Add(new MailAddress(user.Email));
            }
            objeto_mail.Subject = "Labyrinth Administrator notice";
            objeto_mail.Body = message;
            client.Send(objeto_mail);
            return View("Mail");
        }
    }
}