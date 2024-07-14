using AutoMapper;
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
    public class CouseXPackageService : BaseService<CourseXPackage>, ICourseXPackageService
    {
        private readonly ICourseXPackageRepository _repository;

        public CouseXPackageService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.CourseXPackageRepository;
        }
        public async Task<bool> Add(CourseXPackageModel PackageModel)
        {
            var entity = await _repository.GetByCourseIdAndPackageId(PackageModel.CourseId, PackageModel.PackageId);
            if (entity != null) { return false; }
            var Cr = _mapper.Map<CourseXPackage>(PackageModel);
            var cr = await SetBaseEntityToCreateFunc(Cr);
            return await _repository.Add(cr);
        }

        public async Task<bool> Delete(Guid courseId, Guid packageId)
        {
            var entity = await _repository.GetByCourseIdAndPackageId(courseId, packageId);
            if (entity == null)
            {
                return false;
            }

            var Course = _mapper.Map<CourseXPackage>(entity);
            return await _repository.Delete(Course);
        }

        public async Task<List<CourseXPackageModel>?> GetAllByPackageId(Guid packageId)
        {
            var courseXPackages = await _repository.GetAllByPackageId(packageId);
            if (!courseXPackages.Any())
            {
                return null;
            }
            return _mapper.Map<List<CourseXPackageModel>>(courseXPackages);
        }


    }
}
