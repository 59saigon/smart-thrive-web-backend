using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ICourseXPackageRepository : IBaseRepository<CourseXPackage>
    {
        Task<List<CourseXPackage>> GetAllByIdPackage(Guid id);

        Task<CourseXPackage> GetByTwoId(Guid idcourse, Guid idpackage);
     //   Task<bool> DeleteCourseXPackage(Guid idcourse,Guid idpackage);
    }
}