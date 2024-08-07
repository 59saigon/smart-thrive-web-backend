﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using System.Linq;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly STDbContext _context;

        public CourseRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = queryable.Where(m => m.Status == "APPROVED");
            queryable = base.ApplySort(queryable, sortField, sortOrder);
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();
        }

        public async Task<(List<Course>, long)> GetAllPaginationByProviderId(Guid providerId, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = queryable.Where(m => m.ProviderId == providerId);
            queryable = queryable.Where(m => !m.IsDeleted);
            queryable = queryable.Where(m => m.Status == "APPROVED" || m.Status == "NOT REQUEST" || m.Status == "REJECTED");
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            var totalOrigin = queryable.Count();
            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var courses = await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return (courses, totalOrigin);
        }

        public async Task<(List<Course>, long)> Search(Course Course, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);
            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(Course.Status))
                {
                    queryable = queryable.Where(m => m.Status.ToUpper() == Course.Status.ToUpper());
                }
                if (Course.Id != Guid.Empty && Course.Id != null)
                {
                    queryable = queryable.Where(m => m.Id == Course.Id);
                }
                if (!string.IsNullOrEmpty(Course.Code))
                {
                    queryable = queryable.Where(m => m.Code.ToUpper().Contains(Course.Code.ToUpper()));
                }
                if (!string.IsNullOrEmpty(Course.CourseName))
                {
                    queryable = queryable.Where(m => m.CourseName.ToUpper().Contains(Course.CourseName.ToUpper()));
                }

                //    if (!string.IsNullOrEmpty(Course.Description))
                //    {
                //        queryable = queryable.Where(m => m.Description.ToLower().Trim().Contains(Course.Description.ToLower().Trim()));
                //    }

                //    if (!decimal.IsNullOrEmpty(Course.Price))
                //    {
                //        queryable = queryable.Where(m => m.Price == Course.Price);
                //    }

                //    if (user.DOB.HasValue)
                //    {
                //        queryable = queryable.Where(m => m.DOB.Value.Date == user.DOB.Value.Date);
                //    }

                //if (Course.SubjectId != Guid.Empty && Course.SubjectId != null)
                //{
                //    queryable = queryable.Where(m => m.SubjectId == Course.SubjectId);
                //}
            }

            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var courses = await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return (courses, totalOrigin);
        }

        public async Task<List<Course>> SearchCourse(string name)
        {
            var queryable = base.GetQueryable(x => x.CourseName.StartsWith(name) || x.Id.Equals(name));

            if (queryable.Any())
            {
                queryable = queryable.Where(x => !x.IsDeleted);
            }

            if (queryable.Any())
            {
                var results = await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

                return results;
            }

            return null;
        }

        public async new Task<Course?> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var user = await query.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .SingleOrDefaultAsync();

            return user;
        }

        public async new Task<List<Course>> GetAllByProviderId(Guid providerId)
        {
            var query = GetQueryable(m => m.ProviderId == providerId);
            query = query.Where(m => !m.IsDeleted);
            var providers = await query.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return providers;
        }

        public async Task<(List<Course>, long)> GetAllPaginationByListId(List<Guid> guids, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                queryable = queryable.Where(x => guids.Contains(x.Id));
            }
            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var courses = await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return (courses, totalOrigin);
        }

        public async Task<List<Course>> GetAllExceptListId(List<Guid> guids)
        {
            var queryable = GetQueryable();

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                queryable = queryable.Where(x => !guids.Contains(x.Id));
            }

            if (queryable.Any())
            {
                queryable = queryable.Where(x => x.Status == "APPROVED" && x.IsActive == true);
            }

            var courses = await queryable.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return courses;
        }

        public async Task<List<Course>> GetAllPendingStatus()
        {
            var query = GetQueryable(m => m.Status == "Pending");
            var providers = await query.Include(m => m.Sessions)
                .Include(m => m.CourseXPackages)
                .Include(m => m.Subject)
                .Include(m => m.Provider)
                .ToListAsync();

            return providers;
        }
    }
}
