﻿using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        public Task<List<Course>> SearchCourse(string name);
        Task<List<Course>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<List<Course>> GetAllPendingStatus();
        Task<(List<Course>, long)> Search(Course Course, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<(List<Course>, long)> GetAllPaginationByListId(List<Guid> guids, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<Course?> GetById(Guid id);
        Task<List<Course>> GetAllByProviderId(Guid providerId);
        Task<(List<Course>, long)> GetAllPaginationByProviderId(Guid providerId, int pageNumber, int pageSize, string sortField, int sortOrder);
        Task<List<Course>> GetAllExceptListId(List<Guid> guids);
    }
}
