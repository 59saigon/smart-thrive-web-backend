using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindUsernameOrEmail(User user);
        Task<List<User>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<User> GetById(Guid id);
        Task<(List<User>, long)> Search(User user, int pageNumber, int pageSize, string sortField, int sortOrder);
    }
}
