using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using System.Linq;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class PackageRepository : BaseRepository<Package>, IPackageRepository
    {
        private readonly STDbContext _context;

        public PackageRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Package>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable
                .Include(m => m.Student)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Orders)
                .ToListAsync();
        }

        public async Task<(List<Package>, long)> Search(Package Package, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(Package.PackageName))
                {
                    queryable = queryable.Where(m => m.PackageName.ToLower().Trim().Contains(Package.PackageName.ToLower().Trim()));
                }

                if (Package.IsActive.HasValue)
                {
                    queryable = queryable.Where(m => m.IsActive == Package.IsActive);
                }

                if (Package.StudentId != Guid.Empty && Package.StudentId != null)
                {
                    queryable = queryable.Where(m => m.StudentId == Package.StudentId);
                }
            }
            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var pacakges = await queryable
                .Include(m => m.Student)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Orders)
                .ToListAsync();

            return (pacakges, totalOrigin);
        }

        public async Task<Package?> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var user = await query
                .Include(m => m.Student)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Orders)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}
