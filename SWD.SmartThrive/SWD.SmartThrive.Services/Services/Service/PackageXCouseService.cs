﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Repository;
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
    public class PackageXCouseService : BaseService<CourseXPackage>, IPackageXCourseService
    {
        private readonly ICourseXPackageRepository _repository;

        public PackageXCouseService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.CourseXPackageRepository;
        }
        public async Task<bool> Add(CourseXPackageModel PackageModel)
        {
            var entity = await _repository.GetByTwoId(PackageModel.CourseId, PackageModel.PackageId);
            if(entity != null) { return false; }
            var Cr = _mapper.Map<CourseXPackage>(PackageModel);
            var cr = await SetBaseEntityToCreateFuncMany(Cr);
            return await _repository.Add(cr);
        }

        public async Task<bool> Delete(Guid idCourse, Guid idPackage)
        {
            var entity = await _repository.GetByTwoId(idCourse,idPackage);
            if (entity == null)
            {
                return false;
            }

            var Course = _mapper.Map<CourseXPackage>(entity);
            return await _repository.Delete(Course);
        }

        public async Task<List<CourseXPackageModel>> GetAllByIdPackage(Guid id)
        {
            var cr = await _repository.GetAllByIdPackage(id);
            if(cr.Any())
            {
                List<CourseXPackageModel> s =  _mapper.Map<List<CourseXPackageModel>>(cr);
                return s;

            }
            return null;
         }

      
    }
}
