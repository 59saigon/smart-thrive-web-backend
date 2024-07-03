using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<List<Student>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Student>, long)> Search(Student student, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<List<Student>> GetStudentsByUserId(Guid id);
        Task<Student> GetById(Guid id);

    }
}
