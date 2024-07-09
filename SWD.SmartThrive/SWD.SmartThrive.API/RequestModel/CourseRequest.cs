using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.API.RequestModel
{
    public class CourseRequest : BaseRequest
    {
        public Guid? SubjectId { get; set; }

        public Guid? ProviderId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? Code { get; set; }

        public string? CourseName { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? SoldCourses { get; set; }

        public int? TotalSlots { get; set; }

        public int? TotalSessions { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
