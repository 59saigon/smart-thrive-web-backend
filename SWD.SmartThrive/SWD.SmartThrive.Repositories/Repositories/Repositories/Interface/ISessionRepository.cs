using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<List<Session>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Session>, long)> Search(Session Session, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<Session?> GetById(Guid id);
        Task<List<Session>> GetAllByCourseId(Guid courseId);

    }
}
