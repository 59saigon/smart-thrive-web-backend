﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly STDbContext _context;

        public CourseRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCourse(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable.ToListAsync();
        }

        public async Task<(List<Course>, long)> GetAllCourseSearch(Course Course, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = base.ApplySort(sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
            //    if (!string.IsNullOrEmpty(Course.CourseName))
            //    {
            //        queryable = queryable.Where(m => m.CourseName.ToLower().Trim() == Course.CourseName.ToLower().Trim());
            //    }

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

                if (Course.IsActive.HasValue)
                {
                    queryable = queryable.Where(m => m.IsActive == Course.IsActive);
               }
                if (Course.IsApproved.HasValue)
                {
                    queryable = queryable.Where(m => m.IsApproved == Course.IsApproved);
                }


                if (Course.ProviderId != Guid.Empty && Course.LocationId != null)
               {
                    queryable = queryable.Where(m => m.ProviderId == Course.ProviderId);
               }

                if (Course.LocationId != Guid.Empty && Course.LocationId != null)
                {
                    queryable = queryable.Where(m => m.LocationId == Course.LocationId);
                }

                if (Course.SubjectId != Guid.Empty && Course.SubjectId != null)
                {
                    queryable = queryable.Where(m => m.SubjectId == Course.SubjectId);
                }
            }
            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var courses = await queryable.ToListAsync();

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
                var results = await queryable.Include(x => x.Provider).ToListAsync();

                return results;
            }

            return null; 
        }


    }
}
