using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SmartThrive.Repositories.Data.Entities
{
    [Table("Package")]
    public class Package : BaseEntity
    {
        public Guid StudentId { get; set; }

        public string PackageName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? QuantityCourse { get; set; }

        public decimal TotalPrice { get; set; }

        public bool? IsActive { get; set; }

        public virtual Student? Student { get; set; }

        public virtual ICollection<CourseXPackage>? CourseXPackages { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
    }
}
