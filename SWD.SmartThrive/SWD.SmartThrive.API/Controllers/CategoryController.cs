using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.ResponseModel;
using SWD.SmartThrive.API.SearchRequest;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _service.GetAll();
                return categories switch
                {
                    null => Ok(new ItemListResponse<CategoryModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CategoryModel>(ConstantMessage.Success, categories))
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
                var categories = await _service.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _service.GetTotalCount();

                return categories switch
                {
                    null => Ok(new PaginatedListResponse<CategoryModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<CategoryModel>(ConstantMessage.Success, categories, totalOrigin,
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
                var category = await _service.GetById(id);

                return category switch
                {
                    null => Ok(new ItemResponse<CategoryModel>(ConstantMessage.NotFound, null)),
                    not null => Ok(new ItemResponse<CategoryModel>(ConstantMessage.Success, category))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllUserSearch(PaginatedRequest<CategorySearchRequest> paginatedRequest)
        {
            try
            {
                var category = _mapper.Map<CategoryModel>(paginatedRequest.Result);
                var categories = await _service.Search(category, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);;

                return categories.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<CategoryModel>(ConstantMessage.NotFound, categories.Item1, categories.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<CategoryModel>(ConstantMessage.Success, categories.Item1, categories.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(CategoryRequest request)
        {
            try
            {
                bool isSuccess = await _service.Add(_mapper.Map<CategoryModel>(request));
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var isUser = await _service.Delete(id);

                    return isUser switch
                    {
                        true => Ok(new BaseResponse(isUser, ConstantMessage.Success)),
                        _ => Ok(new BaseResponse(isUser, ConstantMessage.Fail))
                    };
                }
                else
                {
                    return BadRequest("It's not empty");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(CategoryRequest request)
        {
            try
            {
                bool isSuccess = await _service.Update(_mapper.Map<CategoryModel>(request));
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
