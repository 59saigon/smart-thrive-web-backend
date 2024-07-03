using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IPackageService
    {
        Task<bool> AddPackage(PackageModel PackageModel);
        Task<bool> DeletePackage(Guid id);
        Task<List<PackageModel>> GetAllPackage();
        Task<PackageModel> GetPackage(Guid id);
        Task<bool> UpdatePackage(PackageModel PackageModel);

        public Task<List<PackageModel>?> GetAllPagination(int pageNumber, int pageSize, string orderBy);

        public Task<List<PackageModel>> GetAllPackageByStudentId( Guid idStudent);

        public Task<(List<PackageModel>?, long)> GetAllPackageSearch(PackageModel packageModel, int pageNumber, int pageSize, string orderBy);

        public Task<long> GetTotalCount();
    }
}
