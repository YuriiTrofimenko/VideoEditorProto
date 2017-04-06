using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoEditorProto.Domain.Abstract;

namespace VideoEditorProto.Controllers
{
    public class EditorController : Controller
    {
        private IProjectRepository mRepository;

        public EditorController(IProjectRepository _projectRepository) {

            mRepository = _projectRepository;
        }

        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult createProject()
        {
            var result =
                from item in mRepository.Project
                select new {item.Id, item.AudioCodecs, item.VideoCodecs };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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