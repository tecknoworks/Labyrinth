﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labyrinth.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View("Chat");
        }
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult PrivateChat()
        {
           return View();
           // return Redirect("Views/Chat/PrivateChat.html");
        }
    }
}