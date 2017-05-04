using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoEditorProto.Domain;
using VideoEditorProto.Domain.Abstract;
using VideoEditorProto.Utils;
using VideoLib;

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
        public JsonResult CreateProject(string _projectId)
        {
            //TODO добавить поля name и description в модель "Проект",
            //поле version - во все модели (для синхронизации клиента и сервера)
            Project newProject = new Project()
            {
                Id = "1",
                IdUser = "1",
                Width = 600,
                Height = 480
            };
            var result = new { result = mRepository.SaveProject(newProject) };
            
            return Json (result, JsonRequestBehavior.AllowGet);
        }

        //localhost:50015/editor/getProjectById?_projectId=1
        [HttpGet]
        public JsonResult GetProjectById(string _projectId)
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
        public JsonResult CreateProject()
        {
            string newProjectId = IdCreator.createProjectGuid();

            Project newProject = new Domain.Project()
            {
                Id = newProjectId,
                Name = Request["name"],
                IdUser = Request["userid"],
                Width = Decimal.Parse(Request["width"]),
                Height = Decimal.Parse(Request["height"])
            };
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
        public JsonResult GetProjectNamesByUserId(string _userId)
        {
            var result =
                from projectItem in mRepository.Project
                where (projectItem.IdUser == _userId)
                select new { id = projectItem.Id, name = projectItem.Name };
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetProjectsByUserId(string _userId)
        {
            var result =
                from projectItem in mRepository.Project
                where (projectItem.IdUser == _userId)
                select new { projectItem.Id, projectItem.User, projectItem.Width, projectItem.Height };
            return Json(result);
        }

        //Обработка изменений на слое монтажной последовательности
        [HttpPost]
        /*public JsonResult processLayoutChange(
            int _begin
            , int _end
            , string _projectId
            , string _layerId
            , string[] _rowIds)*/
        public JsonResult ProcessLayoutChange()
        {

            String fileNamesString = Request["file_names"];

            string[] fileNamesArray = fileNamesString.Split(',');

            Console.WriteLine(fileNamesArray);

            //Проверяем, есть ли в БД слой с таким ИД
            /*var layer =
                (Layer)
                from layerItem in mRepository.Layers
                where (layerItem.Id == _layerId)
                select new Layer()
                {
                    Id = layerItem.Id,
                    IdProject = layerItem.Id
                };

            if (layer == null)
            {
                string newProjectId = IdCreator.createProjectGuid();
                //Если нет - создаем
                Layer newLayer = new Layer()
                {
                    Id = IdCreator.createLayerGuid(),
                    IdProject = _projectId
                };
                layer = mRepository.SaveLayer(newLayer);
            }*/
            

            List<ResourceModel> resModelList = new List<ResourceModel>();
            foreach (string fileName in fileNamesArray)
            {
                ResourceModel rm = new ResourceModel();
                rm.fileName = fileName;
                resModelList.Add(rm);
            }
            //
            String inputPath = Server.MapPath("~/Uploads/");
            //
            String outputPath = Server.MapPath("~/Downloads/");

            VideoLib.VideoProcessor.processLayoutChange(resModelList, 0, 0, inputPath, outputPath);
            return Json(fileNamesArray);
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