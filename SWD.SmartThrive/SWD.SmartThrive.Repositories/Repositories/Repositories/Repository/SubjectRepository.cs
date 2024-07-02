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

        public IQueryable<Subject> GetQueryablePaginationWithSortField(string sortField)
        {
            // Sắp xếp trước 
            var queryable = base.GetQueryable();

            if (queryable.Any())
            {
                switch (sortField.ToLower())
                {
                    case "subjectname":
                        queryable = queryable.OrderBy(o => o.SubjectName);
                        break;
                    default:
                        queryable = queryable.OrderBy(o => o.Id);
                        break;
                }
            }
            return queryable;
        }

        public async Task<List<Subject>> GetAllPaginationWithOrder(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.Include(m => m.Category).Include(m => m.Courses).ToListAsync();
        }

        public async Task<(List<Subject>, long)> Search(Subject subject, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

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

        public async Task<List<Subject>> GetByCategoryId(Guid id)
        {
            var queryable = base.GetQueryable(m => m.CategoryId == id);

            return await queryable.Include(m => m.Category).Include(m => m.Courses)
                .ToListAsync(); ;
        }
    }
}
