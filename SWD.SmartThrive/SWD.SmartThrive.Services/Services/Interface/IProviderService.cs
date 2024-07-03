using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IProviderService
    {
        public Task<bool> Add(ProviderModel model);
        public Task<bool> Delete(ProviderModel model);
        public Task<bool> Update(ProviderModel model);
        public Task<List<ProviderModel>> GetAll();
        public Task<List<ProviderModel>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        public Task<ProviderModel> GetById(Guid id);
        public Task<(List<ProviderModel>?, long)> Search(ProviderModel providerModel, int pageNumber, int pageSize, string sortField, int sortOrder);
        public Task<long> GetTotalCount();

    }
}
