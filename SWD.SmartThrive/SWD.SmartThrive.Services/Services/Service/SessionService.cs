﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class SessionService : BaseService<Session>, ISessionService
    {
        private readonly ISessionRepository _repository;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.SessionRepository;
        }

        public async Task<bool> AddSession(SessionModel SessionModel)
        {
            var Session = _mapper.Map<Session>(SessionModel);
            var session = await SetBaseEntityToCreateFunc(Session);
            return await _repository.Add(session);
        }

        public async Task<bool> UpdateSession(SessionModel sessionModel)
        {
            var entity = await _repository.GetById(sessionModel.Id);

            if (entity == null)
            {
                return false;
            }
            _mapper.Map(sessionModel, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            return await _repository.Update(entity);
        }

        public async Task<bool> DeleteSession(Guid id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                return false;
            }

            var Session = _mapper.Map<Session>(entity);
            return await _repository.Delete(Session);
        }

        public async Task<List<SessionModel>> GetAllSession()
        {
            var sessions = await _repository.GetAll();

            if (!sessions.Any())
            {
                return null;
            }

            return _mapper.Map<List<SessionModel>>(sessions);
        }

        public async Task<SessionModel> GetSession(Guid id)
        {
            var Session = await _repository.GetById(id);

            if (Session == null)
            {
                return null;
            }

            return _mapper.Map<SessionModel>(Session);
        }
    }
}
