using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Category>, long)> Search(Category category, int pageNumber, int pageSize, string sortField, int sortOrder);

    }
}
