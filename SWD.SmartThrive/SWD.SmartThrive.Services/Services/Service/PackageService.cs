using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class PackageService : BaseService<Package>, IPackageService
    {
        private readonly IPackageRepository _repository;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.PackageRepository;
        }

        public async Task<bool> Add(PackageModel PackageModel)
        {
            var Package = _mapper.Map<Package>(PackageModel);
            var package = await SetBaseEntityToCreateFunc(Package);
            return await _repository.Add(package);
        }

        public async Task<bool> Update(PackageModel packageModel)
        {
            var entity = await _repository.GetById(packageModel.Id);

            if (entity == null)
            {
                return false;
            }
            _mapper.Map(packageModel, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            return await _repository.Update(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                return false;
            }

            var Package = _mapper.Map<Package>(entity);
            return await _repository.Delete(Package);
        }

        public async Task<List<PackageModel>> GetAll()
        {
            var Packages = await _repository.GetAll();

            if (!Packages.Any())
            {
                return null;
            }

            return _mapper.Map<List<PackageModel>>(Packages);
        }

        public async Task<PackageModel?> GetById(Guid id)
        {
            var Package = await _repository.GetById(id);

            if (Package == null)
            {
                return null;
            }

            return _mapper.Map<PackageModel>(Package);
        }

        public async Task<List<PackageModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
            {
                var pacakges = await _repository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);

                if (!pacakges.Any())
                {
                    return null;
                }

                return _mapper.Map<List<PackageModel>>(pacakges);

            }

        
        public async Task<(List<PackageModel>?, long)> Search(PackageModel packageModel, int pageNumber, int pageSize, string sortField, int sortOrder)
            {
            var packages = _mapper.Map<Package>(packageModel);
            var packageWithTotalOrigin = await _repository.Search(packages, pageNumber, pageSize, sortField, sortOrder);

            if (!packageWithTotalOrigin.Item1.Any())
            {
                return (null, packageWithTotalOrigin.Item2);
            }
            var courseModels = _mapper.Map<List<PackageModel>>(packageWithTotalOrigin.Item1);

            return (courseModels, packageWithTotalOrigin.Item2);
        }
        
    }
}
