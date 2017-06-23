using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoEditorProto.Domain;
using VideoEditorProto.Domain.Abstract;
using VideoEditorProto.Utils;

namespace VideoEditorProto.Controllers
{
    public class DefaultController : Controller
    {

        private IProjectRepository mRepository;

        public DefaultController(IProjectRepository _projectRepository)
        {
            mRepository = _projectRepository;
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateUser()
        {
            string newUserId = IdCreator.createUserGuid();
            string encodedPassword = StringsEncoder.MD5Hash(Request["password"]);

            User newUser = new Domain.User()
            {
                Id = newUserId,
                Name = Request["name"],
                Email = Request["email"],
                Password = encodedPassword
            };
            var result = mRepository.SaveUser(newUser);
            //from user in mRepository.User
            //where (user.Id == newUserId)
            //select new { item.Id, item.User, item.Width, item.Height };

            return Json(
                new
                {
                    id = result.Id
                    ,
                    name = result.Name
                    ,
                    email = result.Email
                    ,
                    password = result.Password
                });
        }

        [HttpPost]
        public JsonResult GetUser()
        {
            string email = Request["email"];//do not remove this tmp variable!
            string encodedPassword = StringsEncoder.MD5Hash(Request["password"]);

            var user =
                from userItem in mRepository.User
                where (userItem.Email == email
                        && userItem.Password == encodedPassword)
                select new
                {
                    id = userItem.Id
                    ,
                    name = userItem.Name
                    ,
                    email = userItem.Email
                    ,
                    password = userItem.Password
                };

            return Json(user);
        }

        [HttpPost]
        public String SendMessage()
        {
            //Заменить "email_to" на адрес почтового ящика админа сайта
            //Заменить "email_from" на адрес почтового ящика, который будет от формы сайта отправлять письма админу
            //Заменить "email_from_password" на пароль от второго ящика
            Boolean result = Mailer.sendMessage(
                "smtp.google.com"
                //, "email_to"
                //, "email_from"
                , "test@test.ua"
                , "test@test.ua"
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