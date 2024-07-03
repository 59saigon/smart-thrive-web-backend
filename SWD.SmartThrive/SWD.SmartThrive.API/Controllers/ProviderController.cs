using AutoMapper;
using Azure.Core;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.ResponseModel;
using SWD.SmartThrive.API.SearchRequest;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using System.Drawing.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/provider")]
    [ApiController]
    [Authorize]
    public class ProviderController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProviderController(IUserService userService, IMapper mapper, IProviderService providerService)
        {
            _userService = userService;
            _mapper = mapper;
            _providerService = providerService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var providers = await _providerService.GetAll();

                return providers switch
                {
                    null => Ok(new ItemListResponse<ProviderModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<ProviderModel>(ConstantMessage.Success, providers))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-all-pagination")]
        public async Task<IActionResult> GetAllPagination(PaginatedRequest paginatedRequest)
        {
            try
            {
                var providers = await _providerService.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _providerService.GetTotalCount();

                return providers switch
                {
                    null => Ok(new PaginatedListResponse<ProviderModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<ProviderModel>(ConstantMessage.Success, providers, totalOrigin,
                                        paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var model = await _providerService.GetById(id);

                return model switch
                {
                    null => Ok(new ItemResponse<ProviderModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<ProviderModel>(ConstantMessage.Success, model))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllUserSearch(PaginatedRequest<ProviderSearchRequest> paginatedRequest)
        {
            try
            {
                var provider = _mapper.Map<ProviderModel>(paginatedRequest.Result);
                var providers = await _providerService.Search(provider, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return providers.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<ProviderModel>(ConstantMessage.NotFound, providers.Item1, providers.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<ProviderModel>(ConstantMessage.Success, providers.Item1, providers.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(ProviderRequest request)
        {
            try
            {
                bool isSuccess = await _providerService.Add(_mapper.Map<ProviderModel>(request));
                return isSuccess switch
                {
                    true => Ok(new BaseResponse(isSuccess, ConstantMessage.Success)),
                    false => Ok(new BaseResponse(isSuccess, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("delete")]
        public async Task<IActionResult> Delete(ProviderRequest request)
        {
            try
            {
                bool isSuccess = await _providerService.Delete(_mapper.Map<ProviderModel>(request));
                return isSuccess switch
                {
                    true => Ok(new BaseResponse(isSuccess, ConstantMessage.Success)),
                    false => Ok(new BaseResponse(isSuccess, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(ProviderRequest request)
        {
            try
            {
                bool isSuccess = await _providerService.Update(_mapper.Map<ProviderModel>(request));
                return isSuccess switch
                {
                    true => Ok(new BaseResponse(isSuccess, ConstantMessage.Success)),
                    false => Ok(new BaseResponse(isSuccess, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
    }
}
