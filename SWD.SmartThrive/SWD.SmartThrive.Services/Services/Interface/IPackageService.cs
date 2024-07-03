using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IPackageService
    {
        Task<bool> Add(PackageModel PackageModel);
        Task<bool> Delete(Guid id);
        Task<List<PackageModel>> GetAll();
        Task<PackageModel?> GetById(Guid id);
        Task<bool> Update(PackageModel PackageModel);

        public Task<List<PackageModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<(List<PackageModel>?, long)> Search(PackageModel packageModel, int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<long> GetTotalCount();
    }
}
