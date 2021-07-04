using Document_Management_System.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Document_Management_System.Controllers
{
    public class HomeController : Controller
    {
        DocumentManagementEntities context = new DocumentManagementEntities();
        public ActionResult Index()
        {

            return View(HomeIndexViewModel.CreateModel());
        }
        [HttpGet]
        public ActionResult Upload()
        {
            //var cat = TempData["tm_categories"];
            //var cn = TempData["tm_country"];
            //if (cat!=null)
            //{
            //    ViewData["categories"] = cat;
            //    //TempData.Remove("categories");
            //}
            //else
            //{
            //    ViewData["categories"] = GetCategory();
            //}
            //if (cn!=null)
            //{
            //    ViewData["country"] = cn;
            //    //TempData.Remove("country");
            //}
            //else
            //{
            //    ViewData["country"] = GetCountry();
            //}
            return View(HomeUploadViewModel.CreateModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file, Upload upload)
        {
            try
            {
                if (file == null)
                {
                    ModelState.AddModelError(String.Empty, "Please attach file!");
                    return View(HomeUploadViewModel.CreateModel());
                }
                if ((int)upload.CategoryId==0)
                {
                    ModelState.AddModelError(String.Empty, "Please select policy type!");
                    return View(HomeUploadViewModel.CreateModel());
                }
                if (string.IsNullOrEmpty(upload.ResourceName))
                {
                    ModelState.AddModelError(String.Empty, "Please enter file name!");
                    return View(HomeUploadViewModel.CreateModel());
                }
                if (file.ContentLength > 0)
                {
                    if (file.ContentType != "application/pdf")
                    {
                        ModelState.AddModelError(String.Empty, "Please Enter only pdf file");
                        return View(HomeUploadViewModel.CreateModel());
                    }


                }
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _FileExtension = Path.GetExtension(_FileName);
                    _FileName = string.Format(@"{0}", Guid.NewGuid()) + _FileExtension;
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);


                    upload.ResourceUrl = "~/UploadedFiles/" + _FileName;
                    upload.Extension = Path.GetExtension(file.FileName);
                    upload.CreatedDate = DateTime.Now;
                    upload.Status = 1;
                    RecentHistory rh = new RecentHistory();
                    var cat = context.Categories.Where(x => x.CategoryId == upload.CategoryId).SingleOrDefault().CategoryName;
                    rh.HistoryTitle = upload.ResourceName;
                    rh.HistoryDetail = cat + " has been uploaded";
                    rh.HistoryCategory = cat;
                    rh.CreatedDate = DateTime.Now;
                    rh.Status = 1;

                    context.RecentHistories.Add(rh);
                    context.Uploads.Add(upload);
                    context.SaveChanges();
                }
                return View(HomeUploadViewModel.CreateModel());
            }
            catch (System.Data.Entity.Core.UpdateException e)
            {
                return View(HomeUploadViewModel.CreateModel());
            }
        }
        public IEnumerable<SelectListItem> GetCategory()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "--Select--", Value = null });
            foreach (var item in context.Categories.ToList())
            {
                states.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryId.ToString() });
            }
            return states;
        }
        public IEnumerable<SelectListItem> GetCountry()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "--Select--", Value = null });
            foreach (var item in context.Countries.ToList())
            {
                states.Add(new SelectListItem { Text = item.CountryName, Value = item.CountryId.ToString() });
            }
            return states;
        }
        public JsonResult GetEntityByCountry(int countryId)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "--Select--", Value = null });
            foreach (var item in context.Entities.Where(x => x.CountryId == countryId).ToList())
            {
                states.Add(new SelectListItem { Text = item.EntityName, Value = item.EntityId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public JsonResult GetAllEntity()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = null, Value = null });
            foreach (var item in context.Entities.Where(x => x.Status == 1).ToList())
            {
                states.Add(new SelectListItem { Text = item.EntityName, Value = item.EntityId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public JsonResult GetProgramByEntity(int entityId)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "--Select--", Value = null });
            foreach (var item in context.Programs.Where(x => x.EntityId == entityId).ToList())
            {
                states.Add(new SelectListItem { Text = item.ProgramName, Value = item.ProgramId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public JsonResult GetProjectByProgram(int programId)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "--Select--", Value = null });
            foreach (var item in context.Projects.Where(x => x.ProgramId == programId).ToList())
            {
                states.Add(new SelectListItem { Text = item.ProjectName, Value = item.ProjectId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public JsonResult GetAllDesignation()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = null, Value = null });
            foreach (var item in context.Designations.ToList())
            {
                states.Add(new SelectListItem { Text = item.DesignationName, Value = item.DesignationId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public ActionResult Search(string searchTerm, int? categoryId, int? countryId, int? entityId, int? programId, int? projectId, string tag)
        {
            ViewBag.categories = context.Categories.ToList();
            ViewBag.country = context.Countries.ToList();
            searchTerm = searchTerm == "" ? null : searchTerm == string.Empty ? null : searchTerm;
            return View(HomeSearchViewModel.CreateModel(searchTerm, categoryId, countryId, entityId, programId, projectId, tag));
        }
        public ActionResult Test()
        {
            return View();
        }
        public async Task<ActionResult> DownloadAsync(int id)
        {
            int documentId = (int)id;
            var status = true;
            if (status)
            {
                var singleUploadObject = context.Uploads.Where(x => x.UploadId == documentId).SingleOrDefault();
                string filePath = singleUploadObject.ResourceUrl;
                string fileName = singleUploadObject.ResourceName;
                string fileExtension = Path.GetExtension(singleUploadObject.ResourceUrl);
                return await this.ReturnDocumentFileAsync(filePath, fileName, fileExtension);
            }
            else
            {
                TempData["Error"] = "Docuement permission failed";
            }
            return RedirectToAction("Index");
        }

        /*
         * RETURN FILE
         */

        public async Task<FileResult> ReturnDocumentFileAsync(string filePath, string fileName, string fileExtension)
        {
            var getFileName = Path.GetFileName(filePath);
            var path = System.Web.HttpContext.Current.Server.MapPath(filePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), fileName + fileExtension);

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}
            };
        }



    }
}