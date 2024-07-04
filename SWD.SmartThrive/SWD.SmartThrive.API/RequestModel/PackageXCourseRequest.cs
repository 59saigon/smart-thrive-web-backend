using Microsoft.AspNetCore.Mvc;

namespace SWD.SmartThrive.API.RequestModel
{
    public class PackageXCourseRequest
    {
        public Guid CourseId { get; set; }

        public Guid PackageId { get; set; }
    }
}
