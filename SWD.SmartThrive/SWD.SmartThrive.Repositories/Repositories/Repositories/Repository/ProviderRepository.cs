using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Provider>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

            queryable = GetQueryablePagination(queryable, pageNumber, pageSize);    
            
            return await queryable.ToListAsync();
        }

        public async Task<(List<Provider>, long)> Search(Provider provider, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var queryable = GetQueryable();
            queryable = base.ApplySort(queryable, sortField, sortOrder);

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

            var providers = await queryable.Include(m => m.User).Include(m => m.Courses).ToListAsync();

            return (providers, totalOrigin);
        }

        public async Task<Provider> GetById(Guid id)
        {
            var query = GetQueryable(m => m.Id == id);
            var provider = await query
                .Include(m => m.User)
                .Include(m => m.Courses)
                .SingleOrDefaultAsync();

            return provider;
        }
    }
}
