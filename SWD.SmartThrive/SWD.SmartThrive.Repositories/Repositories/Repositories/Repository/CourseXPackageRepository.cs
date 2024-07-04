using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class CourseXPackageRepository : BaseRepository<CourseXPackage>, ICourseXPackageRepository
    {
        private readonly STDbContext context;

        public CourseXPackageRepository(STDbContext _context) : base(_context)
        {
            context = _context;
        }

        //public async Task<bool> DeleteCourseXPackage(Guid idcourse, Guid idpackage)
        //{
        //    var s = await GetByTwoId(idcourse,idpackage);
        //    s.IsDeleted = true;
        //    await Update(s);
        //    return true;
        //}

        public async Task<List<CourseXPackage>> GetAllByIdPackage(Guid id)
        {
            var queryable = await GetQueryable(x => x.PackageId == id).ToListAsync();
            return queryable;
        }

        public async Task<CourseXPackage> GetByTwoId(Guid idcourse, Guid idpackage)
        {
            CourseXPackage s = await context.CourseXPackages.Where(x => x.PackageId == idpackage && x.CourseId == idcourse).SingleOrDefaultAsync();
            return s;
        }
    }
}
