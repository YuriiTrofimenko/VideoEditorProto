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
    public class EditorController : Controller
    {
        private IProjectRepository mRepository;

        public EditorController(IProjectRepository _projectRepository)
        {

            mRepository = _projectRepository;
        }

        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

        /*Действия работы с моделью "Проект"
         для отладки помечены как Get.
         При дальнейшем использовании заменить на Post*/

        //localhost:50015/editor/createProject?_projectId=1
        [HttpGet]
        public JsonResult createProject(string _projectId)
        {
            //TODO добавить поля name и description в модель "Проект",
            //поле version - во все модели (для синхронизации клиента и сервера)
            Project newProject = new Project();
            newProject.Id = "1";
            newProject.IdUser = "1";
            newProject.Width = 600;
            newProject.Height = 480;
            var result = new { result = mRepository.SaveProject(newProject) };
            
            return Json (result, JsonRequestBehavior.AllowGet);
        }

        //localhost:50015/editor/getProjectById?_projectId=1
        [HttpGet]
        public JsonResult getProjectById(string _projectId)
        {
            //TODO add fields: name, description, version
            var result =
                from item in mRepository.Project
                where (item.Id == _projectId)
                select new { item.Id, item.User, item.Width, item.Height };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return new JsonResult { };
        }

        //localhost:50015/editor/getProjectsByUserId?_userId=1
        /*[HttpGet]
        public JsonResult getProjectsByUserId(string _userId)
        {
            var result =
                from item in mRepository.Project
                where (item.IdUser == _userId)
                select new { item.Id, item.User, item.Width, item.Height };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return new JsonResult { };
        }*/

        /*Production POST actions for AJAX*/

        [HttpPost]
        public JsonResult CreateUser()
        {
            string newUserId = IdCreator.createUserGuid();
            string encodedPassword = StringsEncoder.MD5Hash(Request["password"]);

            User newUser = new Domain.User();
            newUser.Id = newUserId;
            newUser.Name = Request["name"];
            newUser.Email = Request["email"];
            newUser.Password = encodedPassword;

            var result = mRepository.SaveUser(newUser);
                //from user in mRepository.User
                //where (user.Id == newUserId)
                //select new { item.Id, item.User, item.Width, item.Height };

            return Json(
                new { id = result.Id
                    ,name = result.Name
                    , email = result.Email
                    , password = result.Password
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
                select new {
                    id = userItem.Id
                    , name = userItem.Name
                    , email = userItem.Email
                    , password = userItem.Password
                };

            return Json(user);
        }

        [HttpPost]
        public JsonResult CreateProject()
        {
            string newProjectId = IdCreator.createProjectGuid();

            Project newProject = new Domain.Project();
            newProject.Id = newProjectId;
            newProject.Name = Request["name"];
            newProject.IdUser = Request["userid"];

            var result = mRepository.SaveProject(newProject);

            return Json(
                new
                {
                    id = result.Id
                    ,name = result.Name
                    ,userid = result.IdUser
                });
        }

        [HttpPost]
        public JsonResult getProjectNamesByUserId(string _userId)
        {
            var result =
                from projectItem in mRepository.Project
                where (projectItem.IdUser == _userId)
                select new { id = projectItem.Id, name = projectItem.Name };
            return Json(result);
        }

        [HttpPost]
        public JsonResult getProjectsByUserId(string _userId)
        {
            var result =
                from projectItem in mRepository.Project
                where (projectItem.IdUser == _userId)
                select new { projectItem.Id, projectItem.User, projectItem.Width, projectItem.Height };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Демо-действие загрузки медиа-файлов на сервер
        [HttpPost]
        public JsonResult Upload()
        {
            string fileName = "";
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Uploads/video/" + fileName));
                    //
                    String inputPath = Server.MapPath("~/Uploads/");
                    //
                    String outputPath = Server.MapPath("~/Downloads/");
                    //
                    VideoLib.VideoProcessor.processResource(fileName, inputPath, outputPath);
                }
            }
            return Json("/Uploads/video/" + fileName);
        }

        //Демо-действие загрузки медиа-файлов на сервер
        [HttpPost]
        public JsonResult CreateRow()
        {
            string fileName = "";
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    //TODO создавать отдельные каталоги для медиафайлов каждого пользователя
                    upload.SaveAs(Server.MapPath("~/Uploads/video/" + fileName));
                    //Устанавливаем путь для загруженных на сервер файлов
                    String inputPath = Server.MapPath("~/Uploads/");
                    //Устанавливаем путь для скачиваемых с сервера файлов
                    String outputPath = Server.MapPath("~/Downloads/");
                    //Берем исходный медиа-файл и создаем его сжатую версию для предпросмотра
                    //TODO создавать отдельные каталоги для preview медиафайлов каждого пользователя
                    VideoLib.VideoProcessor.processResource(fileName, inputPath, outputPath);
                }
            }
            return Json("/Downloads/video/" + fileName);
        }
    }
}