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

        public async Task<List<CourseXPackage>> GetAllByPackageId(Guid id)
        {
            var queryable = GetQueryable(x => x.PackageId == id);
            
            if (queryable.Any())
            {
                queryable = queryable.Include(m => m.Course).Include(m => m.Package);
            }

            return await queryable.ToListAsync();
        }

        public async Task<CourseXPackage?> GetByCourseIdAndPackageId(Guid courseId, Guid packageId)
        {
            CourseXPackage courseXPackage = await context.CourseXPackages.Where(x => x.PackageId == packageId && x.CourseId == courseId).Include(m => m.Course).Include(m => m.Package).SingleOrDefaultAsync();
            return courseXPackage;
        }
    }
}
