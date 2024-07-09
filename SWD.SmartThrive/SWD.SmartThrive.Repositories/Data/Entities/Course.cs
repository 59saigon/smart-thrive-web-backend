using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SmartThrive.Repositories.Data.Entities
{
    [Table("Course")]
    public class Course : BaseEntity
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

        public virtual Location? Location { get; set; }

        public virtual Subject? Subject { get; set; }

        public virtual Provider? Provider { get; set; }

        public virtual ICollection<Session>? Sessions { get; set; }

        public virtual ICollection<CourseXPackage>? CourseXPackages { get; set; }
    }
}
