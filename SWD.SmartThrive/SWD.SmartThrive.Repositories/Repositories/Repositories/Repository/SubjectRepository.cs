using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        private readonly STDbContext _context;
        public SubjectRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Subject>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.Include(m => m.Category).Include(m => m.Courses).ToListAsync();
        }

        public async Task<(List<Subject>, long)> Search(Subject subject, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(subject.SubjectName))
                {
                    queryable = queryable.Where(m => m.SubjectName.ToLower().Trim().StartsWith(subject.SubjectName.ToLower().Trim()));
                }

                if (subject.CategoryId != Guid.Empty && subject.CategoryId != null)
                {
                    queryable = queryable.Where(m => m.CategoryId == subject.CategoryId);
                }
            }

            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var subjects = await queryable.Include(m => m.Category).Include(m => m.Courses).ToListAsync();

            return (subjects, totalOrigin);
        }

        public async Task<List<Subject>> GetSubjectsByCategoryId(Guid categoryId)
        {
            var queryable = base.GetQueryable(m => m.CategoryId == categoryId);

            return await queryable.Include(m => m.Category).Include(m => m.Courses)
                .ToListAsync();
        }

        public async Task<Subject> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var subject = await query
                .Include(m => m.Category)
                .Include(m => m.Courses)
                .SingleOrDefaultAsync();

            return subject;
        }
    }
}
