using SWD.SmartThrive.Repositories.Repositories.Repositories.Model;
using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IOrderService
    {
        Task<bool> Add(OrderModel OrderModel);
        Task<bool> Delete(Guid id);
        Task<List<OrderModel>> GetAll();
        Task<OrderModel?> GetById(Guid id);
        Task<bool> Update(OrderModel OrderModel);

        public Task<List<OrderModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<(List<OrderModel>?, long)> Search(OrderModel ordermodel, int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<long> GetTotalCount();

    }
}
