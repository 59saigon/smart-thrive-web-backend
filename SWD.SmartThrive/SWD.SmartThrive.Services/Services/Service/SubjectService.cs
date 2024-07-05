using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Repository;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class SubjectService : BaseService<Subject>, ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _subjectRepository = unitOfWork.SubjectRepository;
        }
        public async Task<bool> Add(SubjectModel model)
        {
            try
            {
                var subject = _mapper.Map<Subject>(model);
                var setSubject = await SetBaseEntityToCreateFunc(subject);
                return await _subjectRepository.Add(setSubject);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _subjectRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            var subject = _mapper.Map<Subject>(entity);
            return await _subjectRepository.Delete(subject);
        }

        public async Task<(List<SubjectModel>?, long)> Search(SubjectModel model, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var subject = _mapper.Map<Subject>(model);
            var subjectWithTotalOrigin = await _subjectRepository.Search(subject, pageNumber, pageSize, sortField, sortOrder);

            if (!subjectWithTotalOrigin.Item1.Any())
            {
                return (null, subjectWithTotalOrigin.Item2);
            }
            var models = _mapper.Map<List<SubjectModel>>(subjectWithTotalOrigin.Item1);

            return (models, subjectWithTotalOrigin.Item2);
        }

        public async Task<bool> Update(SubjectModel model)
        {
            var entity = await _subjectRepository.GetById(model.Id);

            if (entity == null)
            {
                return false;
            }
            _mapper.Map(model, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            return await _subjectRepository.Update(entity);
        }

        public async Task<List<SubjectModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var subjects = await _subjectRepository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);

            if (!subjects.Any())
            {
                return null;
            }

            return _mapper.Map<List<SubjectModel>>(subjects);
        }

        public async Task<SubjectModel?> GetById(Guid id)
        {
            var subjects = await _subjectRepository.GetById(id);

            if (subjects == null)
            {
                return null;
            }

            return _mapper.Map<SubjectModel>(subjects);
        }

        public async Task<List<SubjectModel>?> GetSubjectsByCategoryId(Guid categoryId)
        {
            try
            {
                var subjects = await _subjectRepository.GetSubjectsByCategoryId(categoryId);

                if (!subjects.Any())
                {
                    return null;
                }

                return _mapper.Map<List<SubjectModel>>(subjects);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SubjectModel>?> GetAll()
        {
            var students = await _subjectRepository.GetAll();

            if (!students.Any())
            {
                return null;
            }

            return _mapper.Map<List<SubjectModel>>(students);
        }
    }
}
