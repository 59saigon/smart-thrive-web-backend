using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface ISubjectService
    {
        Task<bool> Add(SubjectModel model);
        Task<bool> Update(SubjectModel model);
        Task<bool> Delete(Guid id);
        Task<List<SubjectModel>?> GetAll();
        Task<List<SubjectModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<SubjectModel?> GetById(Guid id);
        Task<List<SubjectModel>> GetByCategoryId(Guid id);
        Task<(List<SubjectModel>?, long)> Search(SubjectModel model, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<long> GetTotalCount();
    }
}
