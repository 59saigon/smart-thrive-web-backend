using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SmartThrive.Repositories.Data.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime? DOB { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public bool? Status { get; set; }

        public Guid RoleId { get; set; }

        public Guid? LocationId { get; set; }

        public virtual Role? Role { get; set; }

        public virtual Location? Location { get; set; }

        public virtual Provider? Provider { get; set; }

        public virtual ICollection<Student>? Students { get; set; }
    }
}
