using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Document_Management_System.Models
{
    public class HomeUploadViewModel
    {
        public List<Category> categories { get; set; }
        public List<Country> countries { get; set; }
        public static HomeUploadViewModel CreateModel()
        {
            DocumentManagementEntities db = new DocumentManagementEntities();
            return new HomeUploadViewModel
            {
                categories = db.Categories.ToList(),
                countries = db.Countries.ToList()
            };
        }
    }
}