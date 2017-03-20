using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoEditorProto.Utils;

namespace VideoEditorProto.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public String SendMessage()
        {
            //Заменить "email_to" на адрес почтового ящика админа сайта
            //Заменить "email_from" на адрес почтового ящика, который будет от формы сайта отправлять письма админу
            //Заменить "email_from_password" на пароль от второго ящика
            Boolean result = Mailer.sendMessage(
                "smtp.google.com"
                , "email_to"
                , "email_from"
                , "email_from_password"
                , Request.Params.Get("name")
                , Request.Params.Get("email")
                , Request.Params.Get("subject")
                , Request.Params.Get("message")
            );
            if (result)
            {
                return "ok";
            }
            else
            {
                return "fail";
            }
        }
    }
}