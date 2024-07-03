using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        Task<List<Package>> GetAllPackage(int pageNumber, int pageSize, string orderBy);
        Task<(List<Package>, long)> GetAllPackageSearch(Package Package, int pageNumber, int pageSize, string orderBy);

        Task<List<Package>> GetAllPackageByIdStudent(Guid idstudent);
    }
}
