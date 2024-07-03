using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly STDbContext _context;
        public StudentRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.Include(m => m.Packages).Include(m => m.User)
                .ToListAsync();
        }

        public async Task<(List<Student>, long)> Search(Student student, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(student.StudentName))
                {
                    queryable = queryable.Where(m => m.StudentName.ToLower().Trim().StartsWith(student.StudentName.ToLower().Trim()));
                }

                if (!string.IsNullOrEmpty(student.Gender))
                {
                    queryable = queryable.Where(m => m.Gender.ToLower().Trim().StartsWith(student.Gender.ToLower().Trim()));
                }

                if (student.UserId != Guid.Empty && student.UserId != null)
                {
                    queryable = queryable.Where(m => m.UserId == student.UserId);
                }
            }

            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var providers = await queryable.Include(m => m.Packages).Include(m => m.User)
                .ToListAsync();

            return (providers, totalOrigin);
        }

        public async Task<List<Student>> GetStudentsByUserId(Guid id)
        {
            var queryable = base.GetQueryable(m => m.UserId == id);

            return await queryable.Include(m => m.Packages).Include(m => m.User)
                .ToListAsync();
        }

        public async Task<Student> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var student = await query
                .Include(m => m.User)
                .Include(m => m.Packages)
                .SingleOrDefaultAsync();

            return student;
        }

    }

}
