﻿using SWD.SmartThrive.Repositories.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SWD.SmartThrive.Services.Model
{
    public class CourseModel
    {
        public Guid SubjectId { get; set; }

        public Guid? ProviderId { get; set; }

        public Guid LocationId { get; set; }

        public string? Code { get; set; }

        public string? CourseName { get; set; }

        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
        
        public string? LastUpdatedBy { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? Sold_product { get; set; }

        public int? TotalSlot { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        //public virtual LocationModel Location { get; set; }

        public SubjectModel Subject { get; set; }

        public ProviderModel? Provider { get; set; }

    }
}
