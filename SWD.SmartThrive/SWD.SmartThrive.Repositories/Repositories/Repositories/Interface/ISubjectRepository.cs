using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ISubjectRepository: IBaseRepository<Subject>
    {
        Task<List<Subject>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Subject>, long)> Search(Subject subject, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<List<Subject>> GetByCategoryId(Guid id);
        Task<Subject> GetById(Guid id);

    }
}
