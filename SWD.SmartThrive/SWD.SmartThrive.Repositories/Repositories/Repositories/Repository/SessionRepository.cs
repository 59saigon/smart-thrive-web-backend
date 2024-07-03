using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        private readonly STDbContext _context;
        public SessionRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Session>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.Include(m => m.Course)
                .ToListAsync();
        }

        public async Task<(List<Session>, long)> Search(Session Session, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                //    if (!string.IsNullOrEmpty(Session.SessionName))
                //    {
                //        queryable = queryable.Where(m => m.SessionName.ToLower().Trim() == Session.SessionName.ToLower().Trim());
                //    }

                //    if (!string.IsNullOrEmpty(Session.Description))
                //    {
                //        queryable = queryable.Where(m => m.Description.ToLower().Trim().Contains(Session.Description.ToLower().Trim()));
                //    }

                //    if (!decimal.IsNullOrEmpty(Session.Price))
                //    {
                //        queryable = queryable.Where(m => m.Price == Session.Price);
                //    }

                //    if (user.DOB.HasValue)
                //    {
                //        queryable = queryable.Where(m => m.DOB.Value.Date == user.DOB.Value.Date);
                //    }

                //if (Session.IsActive.HasValue)
                //{
                //    queryable = queryable.Where(m => m.IsActive == Session.IsActive);
                //}
                //if (Session.IsApproved.HasValue)
                //{
                //    queryable = queryable.Where(m => m.IsApproved == Session.IsApproved);
                //}


                //if (Session.ProviderId != Guid.Empty && Session.LocationId != null)
                //{
                //    queryable = queryable.Where(m => m.ProviderId == Session.ProviderId);
                //}

                //if (Session.LocationId != Guid.Empty && Session.LocationId != null)
                //{
                //    queryable = queryable.Where(m => m.LocationId == Session.LocationId);
                //}

                //if (Session.SubjectId != Guid.Empty && Session.SubjectId != null)
                //{
                //    queryable = queryable.Where(m => m.SubjectId == Session.SubjectId);
                //}
            }
            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var sessions = await queryable.Include(m => m.Course)
                .ToListAsync();

            return (sessions, totalOrigin);
        }

        public async Task<Session> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var session = await query.Include(m => m.Course).SingleOrDefaultAsync();

            return session;
        }
    }
}
