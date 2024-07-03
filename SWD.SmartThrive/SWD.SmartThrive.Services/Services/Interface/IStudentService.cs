using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IStudentService
    {
        Task<bool> Add(StudentModel model);
        Task<bool> Update(StudentModel model);
        Task<bool> Delete(Guid id);
        Task<List<StudentModel>?> GetAll();
        Task<List<StudentModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<StudentModel?> GetById(Guid id);
        Task<List<StudentModel>> GetStudentsByUserId(Guid id);
        Task<(List<StudentModel>?, long)> Search(StudentModel model, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<long> GetTotalCount();
    }
}
