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
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.CategoryRepository;
        }
        public async Task<bool> Add(CategoryModel model)
        {
            var category = _mapper.Map<Category>(model);
            var setCategory = await SetBaseEntityToCreateFunc(category);

            return await _repository.Add(setCategory);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _repository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            var category = _mapper.Map<Category>(entity);
            return await _repository.Delete(category);
        }

        public async Task<(List<CategoryModel>?, long)> Search(CategoryModel model, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var category = _mapper.Map<Category>(model);
            var categoryWithTotalOrigin = await _repository.Search(category, pageNumber, pageSize, sortField, sortOrder);

            if (!categoryWithTotalOrigin.Item1.Any())
            {
                return (null, categoryWithTotalOrigin.Item2);
            }
            var models = _mapper.Map<List<CategoryModel>>(categoryWithTotalOrigin.Item1);

            return (models, categoryWithTotalOrigin.Item2);
        }

        public async Task<bool> Update(CategoryModel model)
        {
            var entity = await _repository.GetById(model.Id);

            if (entity == null)
            {
                return false;
            }
            _mapper.Map(model, entity);
            entity = await SetBaseEntityToUpdateFunc(entity);

            return await _repository.Update(entity);
        }

        public async Task<List<CategoryModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var categories = await _repository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);
            
            if (!categories.Any())
            {
                return null;
            }

            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel?> GetById(Guid id)
        {
            var category = await _repository.GetById(id);

            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            var categories = await _repository.GetAll();

            if (!categories.Any())
            {
                return null;
            }

            return _mapper.Map<List<CategoryModel>>(categories);
        }

    }
}
