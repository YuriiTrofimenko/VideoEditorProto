using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoEditorProto.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index()
        {
            return View();
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