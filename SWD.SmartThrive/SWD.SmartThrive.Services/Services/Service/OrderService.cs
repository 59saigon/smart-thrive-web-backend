using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Model;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _repository = unitOfWork.OrderRepository;
        }

        public async Task<bool> Add(OrderModel OrderModel)
        {
            var Order = _mapper.Map<Order>(OrderModel);
            var order = await SetBaseEntityToCreateFunc(Order);
            return await _repository.Add(order);
        }

        public async Task<bool> Update(OrderModel orderModel)
        {
            var entity = await _repository.GetById(orderModel.Id);

            if (entity == null)
            {
                return false;
            }
            _mapper.Map(orderModel, entity);
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

            var Order = _mapper.Map<Order>(entity);
            return await _repository.Delete(Order);
        }

        public async Task<List<OrderModel>> GetAll()
        {
            var Orders = await _repository.GetAll();

            if (!Orders.Any())
            {
                return null;
            }

            return _mapper.Map<List<OrderModel>>(Orders);
        }

        public async Task<OrderModel?> GetById(Guid id)
        {
            var Order = await _repository.GetById(id);

            if (Order == null)
            {
                return null;
            }

            return _mapper.Map<OrderModel>(Order);
        }

        public async Task<List<OrderModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var orders = await _repository.GetAllPagination(pageNumber, pageSize, sortField, sortOrder);

            if (!orders.Any())
            {
                return null;
            }

            return _mapper.Map<List<OrderModel>>(orders);
        }

        public async Task<(List<OrderModel>?, long)> Search(OrderModel ordermodel, int pageNumber, int pageSize, string sortField, int sortOrder)
        {
            var orders= _mapper.Map<Order>(ordermodel);
            var ordersWithTotalOrigin = await _repository.Search(orders, pageNumber, pageSize, sortField, sortOrder);

            if (!ordersWithTotalOrigin.Item1.Any())
            {
                return (null, ordersWithTotalOrigin.Item2);
            }
            var courseModels = _mapper.Map<List<OrderModel>>(ordersWithTotalOrigin.Item1);

            return (courseModels, ordersWithTotalOrigin.Item2);
        }
    }
}
