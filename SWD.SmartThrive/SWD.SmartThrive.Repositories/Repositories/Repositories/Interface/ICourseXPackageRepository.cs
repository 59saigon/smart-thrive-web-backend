using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ICourseXPackageRepository : IBaseRepository<CourseXPackage>
    {
        Task<List<CourseXPackage>> GetAllByPackageId(Guid id);

        Task<CourseXPackage> GetByCourseIdAndPackageId(Guid courseId, Guid packageId);
     //   Task<bool> DeleteCourseXPackage(Guid idcourse,Guid idpackage);
    }
}