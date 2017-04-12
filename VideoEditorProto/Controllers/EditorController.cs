using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoEditorProto.Domain;
using VideoEditorProto.Domain.Abstract;

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
        public JsonResult createProject(int _projectId)
        {
            //TODO добавить поля name и description в модель "Проект",
            //поле version - во все модели (для синхронизации клиента и сервера)
            Project newProject = new Project();
            newProject.Id = 1;
            newProject.IdUser = 1;
            newProject.Width = 600;
            newProject.Height = 480;
            var result = new { result = mRepository.SaveProject(newProject) };
            
            return Json (result, JsonRequestBehavior.AllowGet);
        }

        //localhost:50015/editor/getProjectById?_projectId=1
        [HttpGet]
        public JsonResult getProjectById(int _projectId)
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
        [HttpGet]
        public JsonResult getProjectsByUserId(int _userId)
        {
            var result =
                from item in mRepository.Project
                where (item.IdUser == _userId)
                select new { item.Id, item.User, item.Width, item.Height };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return new JsonResult { };
        }

        //Демо-действие загрузки медиа-файлов на сервер
        [HttpPost]
        public JsonResult Upload()
        {
            /*foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Uploads/video/" + fileName));
                }
            }
            return Json("файл загружен");*/
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
    }
}