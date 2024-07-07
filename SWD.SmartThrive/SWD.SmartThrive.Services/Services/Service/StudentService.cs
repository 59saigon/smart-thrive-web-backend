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
    public class StudentService : BaseService<Student>, IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _studentRepository = unitOfWork.StudentRepository;
        }
        public async Task<bool> Add(StudentModel model)
        {
            try
            {
                model.DOB = model.DOB.Value.ToLocalTime();

                var student = _mapper.Map<Student>(model);
                var setStudent = await SetBaseEntityToCreateFunc(student);
                return await _studentRepository.Add(setStudent);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _studentRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            var student = _mapper.Map<Student>(entity);
            return await _studentRepository.Delete(student);
        }

        public async Task<List<StudentModel>?> GetAll()
        {
            var students = await _studentRepository.GetAll();

            if (!students.Any())
            {
                return null;
            }

            return _mapper.Map<List<StudentModel>>(students);
        }

        public async Task<List<StudentModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var students = await _studentRepository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);

            if (!students.Any())
            {
                return null;
            }

            return _mapper.Map<List<StudentModel>>(students);
        }

        public async Task<StudentModel?> GetById(Guid id)
        {
            var students = await _studentRepository.GetById(id);

            if (students == null)
            {
                return null;
            }

            return _mapper.Map<StudentModel>(students);
        }

        public async Task<List<StudentModel>> GetStudentsByUserId(Guid id)
        {
            try
            {
                return _mapper.Map<List<StudentModel>>(await _studentRepository.GetStudentsByUserId(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<StudentModel>?, long)> Search(StudentModel model, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var student = _mapper.Map<Student>(model);
            var studentsWithTotalOrigin = await _studentRepository.Search(student, pageNumber, pageSize, sortField, sortOrder);

            if (!studentsWithTotalOrigin.Item1.Any())
            {
                return (null, studentsWithTotalOrigin.Item2);
            }
            var models = _mapper.Map<List<StudentModel>>(studentsWithTotalOrigin.Item1);

            return (models, studentsWithTotalOrigin.Item2);
        }

        public async Task<bool> Update(StudentModel model)
        {
            var entity = await _studentRepository.GetById(model.Id);

            if (entity == null)
            {
                return false;
            }

            model.DOB = model.DOB.Value.ToLocalTime();
            _mapper.Map(model, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            return await _studentRepository.Update(entity);
        }
    }
}
