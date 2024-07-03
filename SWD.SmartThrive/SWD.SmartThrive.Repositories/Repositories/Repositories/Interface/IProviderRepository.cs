using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IProviderRepository: IBaseRepository<Provider>
    {
        Task<List<Provider>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Provider>, long)> Search(Provider provider, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<Provider> GetById(Guid id);

    }
}
