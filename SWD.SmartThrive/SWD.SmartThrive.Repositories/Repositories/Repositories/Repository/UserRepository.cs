﻿using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly STDbContext _context;

        public UserRepository(STDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> FindUsernameOrEmail(User user)
        {
            var queryable = base.GetQueryable();
            queryable = queryable.Where(entity => !entity.IsDeleted);

            if (!string.IsNullOrEmpty(user.Username) || !string.IsNullOrEmpty(user.Email))
            {
                queryable = queryable.Where(entity => user.Username.ToLower() == entity.Username.ToLower()
                                            || user.Email.ToLower() == entity.Email.ToLower()
                );
            }

            var result = await queryable
                .Include(m => m.Role)
                .Include(m => m.Provider)
                .Include(m => m.Students).SingleOrDefaultAsync();

            return result;
        }
        
        public async Task<User> GetUserByEmail(User user)
        {
            var queryable = base.GetQueryable();
            queryable = queryable.Where(entity => !entity.IsDeleted);

            if (!string.IsNullOrEmpty(user.Email))
            {
                queryable = queryable.Where(entity => user.Email.ToLower() == entity.Email.ToLower()
                );
            }

            var result = await queryable
                .Include(m => m.Role)
                .Include(m => m.Provider)
                .Include(m => m.Students).SingleOrDefaultAsync();

            return result;
        }

        public async Task<List<User>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            return await queryable
                .Include(m => m.Role)
                .Include(m => m.Students).ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var user = await query.Include(m => m.Students).SingleOrDefaultAsync();

            return user;
        }

        public async Task<(List<User>, long)> Search(User user, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(user.Username))
                {
                    queryable = queryable.Where(m => m.Username.ToLower().Trim() == user.Username.ToLower().Trim());
                }

                if (!string.IsNullOrEmpty(user.FullName))
                {
                    queryable = queryable.Where(m => m.FullName.ToLower().Trim().Contains(user.FullName.ToLower().Trim()));
                }

                if (!string.IsNullOrEmpty(user.Email))
                {
                    queryable = queryable.Where(m => m.Email.ToLower().Trim() == user.Email.ToLower().Trim());
                }

                if (user.DOB.HasValue)
                {
                    queryable = queryable.Where(m => m.DOB.Value.Date == user.DOB.Value.Date);
                }

                if (!string.IsNullOrEmpty(user.Address))
                {
                    queryable = queryable.Where(m => m.Address.ToLower().Trim().Contains(user.Address.ToLower().Trim()));
                }

                if (!string.IsNullOrEmpty(user.Gender))
                {
                    queryable = queryable.Where(m => m.Gender.ToLower().Trim() == user.Gender.ToLower().Trim());
                }

                if (!string.IsNullOrEmpty(user.Phone))
                {
                    queryable = queryable.Where(m => m.Phone.ToLower().Trim().Contains(user.Phone.ToLower().Trim()));
                }

                if (user.Status.HasValue)
                {
                    queryable = queryable.Where(m => m.Status == user.Status);
                }

                if (user.RoleId != Guid.Empty && user.RoleId != null)
                {
                    queryable = queryable.Where(m => m.RoleId == user.RoleId);
                }

            }

            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var users = await queryable
                .Include(m => m.Role)
                .Include(m => m.Students).ToListAsync();

            return (users, totalOrigin);
        }
    }
} 
