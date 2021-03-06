using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Document_Management_System.Models
{
    public class HomeSearchViewModel
    {
        public List<Upload> uploads { get; set; }
        public static HomeSearchViewModel CreateModel(string searchTerm, int? categoryId, int? countryId, int? entityId, int? programId, int? projectId, string tag)
        {
            DocumentManagementEntities db = new DocumentManagementEntities();

            var lstUpload = (from u in db.Uploads
                             join c in db.Categories on u.CategoryId equals c.CategoryId into uc
                             from subc in uc.DefaultIfEmpty()
                             join cn in db.Countries on u.CountryId equals cn.CountryId into ucn
                             from subucn in ucn.DefaultIfEmpty()
                             join e in db.Entities on u.EntityId equals e.EntityId into ue
                             from subue in ue.DefaultIfEmpty()
                             join p in db.Programs on u.ProgramId equals p.ProgramId into up
                             from subup in up.DefaultIfEmpty()
                             join pr in db.Projects on u.ProjectId equals pr.ProjectId into upr
                             from subupr in upr.DefaultIfEmpty()
                             select new {
                                 UploadId = u.UploadId,
                                 CategoryId = u.CategoryId,
                                 CountryId = u.CountryId,
                                 EntityId = u.EntityId,
                                 ProgramId = u.ProjectId,
                                 ProjectId = u.ProjectId,
                                 ResourceUrl = u.ResourceUrl,
                                 Extension = u.Extension,
                                 ResourceName = u.ResourceName,
                                 Tags = u.Tags,
                                 ApprovedDate = u.ApprovedDate,
                                 ApprovedByDesgination = u.ApprovedByDesgination,
                                 ApprovedByName = u.ApprovedByName,
                                 Version = u.Version,
                                 LastReviewedDate = u.LastReviewedDate,
                                 EffectiveFrom = u.EffectiveFrom,
                                 EffectiveTo = u.EffectiveTo,
                                 CreatedBy = u.CreatedBy,
                                 CreatedDate = u.CreatedDate,
                                 CategoryName = subc.CategoryName,
                                 CountryName = subucn == null ? null : subucn.CountryName,
                                 EntityName = subue == null ? null : subue.EntityName,
                                 ProgramName = subup == null ? null : subup.ProgramName,
                                 ProjectName = subupr == null ? null : subupr.ProjectName
                             });
            if (!string.IsNullOrEmpty(searchTerm))
            {
                lstUpload = lstUpload.Where(x => x.ResourceName.Contains(searchTerm));
            }
            if (categoryId.HasValue&& categoryId.Value>0)
            {
                lstUpload = lstUpload.Where(x => x.CategoryId==categoryId.Value);
            }
            if (countryId.HasValue && countryId.Value > 0)
            {
                lstUpload = lstUpload.Where(x => x.CountryId == countryId.Value);
            }
            if (entityId.HasValue && entityId.Value > 0)
            {
                lstUpload = lstUpload.Where(x => x.EntityId == entityId.Value);
            }
            if (programId.HasValue && programId.Value > 0)
            {
                lstUpload = lstUpload.Where(x => x.ProgramId == programId.Value);
            }
            if (projectId.HasValue && projectId.Value > 0)
            {
                lstUpload = lstUpload.Where(x => x.ProjectId == projectId.Value);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                lstUpload = lstUpload.Where(x => x.Tags.Contains(tag));
            }
            var uploadList = lstUpload.ToList();
            List<Upload> uploadToSet = uploadList.Select(x => new Upload
            {
                UploadId = x.UploadId,
                CategoryId = x.CategoryId,
                CountryId = x.CountryId,
                EntityId = x.EntityId,
                ProgramId = x.ProjectId,
                ProjectId = x.ProjectId,
                ResourceUrl = x.ResourceUrl,
                Extension = x.Extension,
                ResourceName = x.ResourceName,
                Tags = x.Tags,
                ApprovedDate = x.ApprovedDate,
                ApprovedByDesgination = x.ApprovedByDesgination,
                ApprovedByName = x.ApprovedByName,
                Version = x.Version,
                LastReviewedDate = x.LastReviewedDate,
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CategoryName = x.CategoryName,
                CountryName = x.CountryName,
                EntityName = x.EntityName,
                ProgramName = x.ProgramName,
                ProjectName = x.ProjectName
            }).ToList();
            return new HomeSearchViewModel
            {
                uploads = uploadToSet
            };
        }
    }
}