using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IPackageXCourseService
    {
        Task<bool> Add(CourseXPackageModel PackageModel);
        Task<bool> Delete(Guid idCourse,Guid idPackage);
        Task<List<CourseXPackageModel>> GetAllByIdPackage(Guid id);
    }
}
