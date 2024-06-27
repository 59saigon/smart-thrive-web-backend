﻿using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Repository
{
    public class ProviderRepository : BaseRepository<Provider>, IProviderRepository
    {
        public ProviderRepository(STDbContext context) : base(context)
        {
        }

        public IQueryable<Provider> GetQueryablePaginationWithOrderBy(string orderBy)
        {
            // Sắp xếp trước 
            var queryable = base.GetQueryable();

            if (queryable.Any())
            {
                switch (orderBy.ToLower())
                {
                    case "website":
                        queryable = queryable.OrderBy(o => o.Website);
                        break;
                    case "companyname":
                        queryable = queryable.OrderBy(o => o.CompanyName);
                        break;
                    default:
                        queryable = queryable.OrderBy(o => o.Id);
                        break;
                }
            }

            return queryable;
        }

        public async Task<List<Provider>> GetAllPaginationWithOrder(int pageNumber, int pageSize, string orderBy)
        {
            var queryable = this.GetQueryablePaginationWithOrderBy(orderBy);

            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);    
            
            return await queryable.ToListAsync();
        }

        public async Task<(List<Provider>, long)> Search(Provider provider, int pageNumber, int pageSize, string orderBy)
        {
            var queryable = this.GetQueryablePaginationWithOrderBy(orderBy);

            // Điều kiện lọc từng bước
            if (queryable.Any())
            {
                if (!string.IsNullOrEmpty(provider.CompanyName))
                {
                    queryable = queryable.Where(m => m.CompanyName.ToLower().Trim().StartsWith(provider.CompanyName.ToLower().Trim()));
                }

                if (!string.IsNullOrEmpty(provider.Website))
                {
                    queryable = queryable.Where(m => m.Website.ToLower().Trim().StartsWith(provider.Website.ToLower().Trim()));
                }
               
                if (provider.UserId != Guid.Empty && provider.UserId != null)
                {
                    queryable = queryable.Where(m => m.UserId == provider.UserId);
                }
            }

            var totalOrigin = queryable.Count();

            // Lọc theo trang
            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);

            var providers = await queryable.ToListAsync();

            return (providers, totalOrigin);
        }
    }
}
