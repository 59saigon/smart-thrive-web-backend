using SWD.SmartThrive.Services.Model;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface ICourseService
    {
        Task<bool> Add(CourseModel CourseModel);
        Task<bool> Delete(Guid id);
        Task<List<CourseModel>> GetAll();
        Task<CourseModel?> GetById(Guid id);
        Task<bool> Update(CourseModel CourseModel);
        public Task<List<CourseModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<(List<CourseModel>?, long)> Search(CourseModel courseModel, int pageNumber, int pageSize, string sortField, int sortOrder);
        public Task<(List<CourseModel>?, long)> GetAllPaginatiomByListId(List<Guid> guids, int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<long> GetTotalCount();
        Task<List<CourseModel>> GetAllByProviderId(Guid providerId);
        Task<(List<CourseModel>?, long)> GetAllPaginationByProviderId(Guid providerId, int pageNumber, int pageSize, string sortField, int sortOrder);
    }
}
