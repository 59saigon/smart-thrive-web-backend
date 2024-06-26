﻿using SWD.SmartThrive.Repositories.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SWD.SmartThrive.Services.Model
{
    public class CourseModel : BaseModel
    {
        public Guid? SubjectId { get; set; }

        public Guid? ProviderId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? Code { get; set; }

        public string? CourseName { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? Sold_product { get; set; }

        public int? TotalSlot { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public LocationModel? Location { get; set; }

        public SubjectModel? Subject { get; set; }

        public ProviderModel? Provider { get; set; }

        public IList<SessionModel>? Sessions { get; set; }

        public IList<CourseXPackageModel>? CourseXPackages { get; set; }

    }
}
