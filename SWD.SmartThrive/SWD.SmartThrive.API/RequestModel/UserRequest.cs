using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.API.RequestModel
{
    public class UserRequest : BaseRequest
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public DateTime? DOB { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public bool? Status { get; set; }

        public Guid? RoleId { get; set; }

        public string? RoleName { get; set; }

        public Guid? LocationId { get; set; }
    }
}
