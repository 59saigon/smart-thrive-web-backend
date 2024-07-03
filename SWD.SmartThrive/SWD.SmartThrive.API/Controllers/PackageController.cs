using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.Services.Services.Interface;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.API.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using SWD.SmartThrive.Services.Services.Service;
using SWD.SmartThrive.API.SearchRequest;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/package")]
    [ApiController]
    [Authorize]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _service;
        private readonly IMapper _mapper;

        public PackageController(IPackageService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var packages = await _service.GetAll();

                return packages switch
                {
                    null => Ok(new ItemListResponse<PackageModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<PackageModel>(ConstantMessage.Success, packages))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-package-by-student-id")]
        public async Task<IActionResult> GetAllbyStudentId(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Ok("Id is empty");
                }
                var packages = await _service.GetAllPackageByStudentId(id);

                return packages switch
                {
                    null => Ok(new ItemListResponse<PackageModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<PackageModel>(ConstantMessage.Success, packages))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var packageModel = await _service.GetById(id);

                return packageModel switch
                {
                    null => Ok(new ItemResponse<PackageModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<PackageModel>(ConstantMessage.Success, packageModel))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPackage(PackageRequest package)
        {
            try
            {
                var isPackage = await _service.Add(_mapper.Map<PackageModel>(package));

                return isPackage switch
                {
                    true => Ok(new BaseResponse(isPackage, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isPackage, ConstantMessage.Fail))
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
                    var isPackage = await _service.Delete(id);

                    return isPackage switch
                    {
                        true => Ok(new BaseResponse(isPackage, ConstantMessage.Success)),
                        _ => Ok(new BaseResponse(isPackage, ConstantMessage.Fail))
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
        public async Task<IActionResult> Update(PackageRequest package)
        {
            try
            {
                var packageModel = _mapper.Map<PackageModel>(package);

                var isPackage = await _service.Update(packageModel);

                return isPackage switch
                {
                    true => Ok(new BaseResponse(isPackage, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isPackage, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllPackageSearch(PaginatedRequest<PackageSearchRequest> paginatedRequest)
        {
            try
            {
                var package = _mapper.Map<PackageModel>(paginatedRequest.Result);
                var packages = await _service.Search(package, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return packages.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<PackageModel>(ConstantMessage.NotFound, packages.Item1, packages.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<PackageModel>(ConstantMessage.Success, packages.Item1, packages.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-pagination")]
        public async Task<IActionResult> GetAllPackages(PaginatedRequest paginatedRequest)
        {
            try
            {
                var packages = await _service.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _service.GetTotalCount();
                return packages switch
                {
                    null => Ok(new PaginatedListResponse<PackageModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<PackageModel>(ConstantMessage.Success, packages, totalOrigin, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

    }
}
