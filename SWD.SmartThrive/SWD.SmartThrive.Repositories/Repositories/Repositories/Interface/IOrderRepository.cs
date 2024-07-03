using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Model;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
    //    public Task<List<Course>> SearchCourse(string name);
        Task<List<Order>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Order>, long)> Search(Order Order, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<Order> GetById(Guid id);

    }
}
