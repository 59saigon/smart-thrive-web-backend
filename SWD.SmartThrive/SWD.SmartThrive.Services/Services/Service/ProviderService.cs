using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
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
    public class ProviderService : BaseService<Provider>, IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        public ProviderService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _providerRepository = unitOfWork.ProviderRepository;
        }


        public async Task<bool> Add(ProviderModel model)
        {
            try
            {
                var provider = _mapper.Map<Provider>(model);
                var setProvider = await SetBaseEntityToCreateFunc(provider);
                return await _providerRepository.Add(setProvider);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(ProviderModel model)
        {
            try
            {
                return await _providerRepository.Delete(_mapper.Map<Provider>(model));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProviderModel>> GetAll()
        {
            try
            {
                var providers = await _providerRepository.GetAll();
                return _mapper.Map<List<ProviderModel>>(providers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProviderModel>> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            try
            {
                return _mapper.Map<List<ProviderModel>>(await _providerRepository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProviderModel?> GetById(Guid id)
        {
            var package = await _providerRepository.GetById(id);

            if (package == null)
            {
                return null;
            }

            return _mapper.Map<ProviderModel>(package);
        }

        public async Task<(List<ProviderModel>?, long)> Search(ProviderModel providerModel, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var provider = _mapper.Map<Provider>(providerModel);
            var providersWithTotalOrigin = await _providerRepository.Search(provider, pageNumber, pageSize, sortField, sortOrder);

            if (!providersWithTotalOrigin.Item1.Any())
            {
                return (null, providersWithTotalOrigin.Item2);
            }
            var providerModels = _mapper.Map<List<ProviderModel>>(providersWithTotalOrigin.Item1);

            return (providerModels, providersWithTotalOrigin.Item2);
        }

        public async Task<bool> Update(ProviderModel providerModel)
        {
            try
            {
                var entity = await _providerRepository.GetById(providerModel.Id);

                if (entity == null)
                {
                    return false;
                }
                _mapper.Map(providerModel, entity);
                entity = await SetBaseEntityToUpdateFunc(entity);

                return await _providerRepository.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
