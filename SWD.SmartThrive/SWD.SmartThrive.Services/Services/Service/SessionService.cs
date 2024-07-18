using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Repository;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class SessionService : BaseService<Session>, ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _sessionRepository = unitOfWork.SessionRepository;
        }

        public async Task<bool> Add(SessionModel sessionModel)
        {
            sessionModel.LearnDate = sessionModel.LearnDate.Value.ToLocalTime();

            var session = _mapper.Map<Session>(sessionModel);
            var setSession = await SetBaseEntityToCreateFunc(session);

            return await _sessionRepository.Add(setSession);
        }

        public async Task<bool> Update(SessionModel sessionModel)
        {
            var entity = await _sessionRepository.GetById(sessionModel.Id);

            if (entity == null)
            {
                return false;
            }
            sessionModel.LearnDate = sessionModel.LearnDate.Value.ToLocalTime();
            _mapper.Map(sessionModel, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            var session = _mapper.Map<Session>(sessionModel);
            return await _sessionRepository.Update(session);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _sessionRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            var session = _mapper.Map<Session>(entity);
            return await _sessionRepository.Delete(session);
        }

        public async Task<List<SessionModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var sessions = await _sessionRepository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);

            if (!sessions.Any())
            {
                return null;
            }

            return _mapper.Map<List<SessionModel>>(sessions);
        }

        public async Task<List<SessionModel>?> GetAll()
        {
            var sessions = await _sessionRepository.GetAll();

            if (!sessions.Any())
            {
                return null;
            }

            return _mapper.Map<List<SessionModel>>(sessions);
        }

        public async Task<(List<SessionModel>?, long)> Search(SessionModel sessionModel, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var session = _mapper.Map<Session>(sessionModel);
            var sessionsWithTotalOrigin = await _sessionRepository.Search(session, pageNumber, pageSize, sortField, sortOrder);

            if (!sessionsWithTotalOrigin.Item1.Any())
            {
                return (null, sessionsWithTotalOrigin.Item2);
            }
            var sessionModels = _mapper.Map<List<SessionModel>>(sessionsWithTotalOrigin.Item1);

            return (sessionModels, sessionsWithTotalOrigin.Item2);
        }

        public async Task<SessionModel?> GetById(Guid id)
        {
            var session = await _sessionRepository.GetById(id);

            if (session == null)
            {
                return null;
            }

            return _mapper.Map<SessionModel>(session);
        }

        public async Task<List<SessionModel>?> GetAllByCourseId(Guid courseId)
        {

            var sessions = await _sessionRepository.GetAllByCourseId(courseId);

            if (!sessions.Any())
            {
                return null;
            }

            return _mapper.Map<List<SessionModel>>(sessions);
        }
        
        public async Task<List<SessionModel>?> GetAllByCourseIdForProvider(Guid courseId)
        {

            var sessions = await _sessionRepository.GetAllByCourseIdForProvider(courseId);

            if (!sessions.Any())
            {
                return null;
            }

            return _mapper.Map<List<SessionModel>>(sessions);
        }
    }
}
