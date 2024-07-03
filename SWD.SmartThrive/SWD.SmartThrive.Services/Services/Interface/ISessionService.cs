using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface ISessionService
    {
        Task<bool> Add(SessionModel model);
        Task<bool> Update(SessionModel model);
        Task<bool> Delete(Guid id);
        Task<List<SessionModel>?> GetAll();
        Task<List<SessionModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<SessionModel?> GetById(Guid id);
        Task<(List<SessionModel>?, long)> Search(SessionModel model, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<long> GetTotalCount();
    }
}
