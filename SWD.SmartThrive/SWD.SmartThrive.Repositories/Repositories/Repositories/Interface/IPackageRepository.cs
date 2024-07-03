using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        Task<List<Package>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Package>, long)> Search(Package Package, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<Package?> GetById(Guid id);

    }
}
