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
using SWD.SmartThrive.Services.Services.Service;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/subject")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _service;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectService subjectService, IMapper mapper)
        {
            _service = subjectService;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subjects = await _service.GetAll();
                return subjects switch
                {
                    null => Ok(new ItemListResponse<SubjectModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<SubjectModel>(ConstantMessage.Success, subjects))
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
                var subjects = await _service.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _service.GetTotalCount();

                return subjects switch
                {
                    null => Ok(new PaginatedListResponse<SubjectModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<SubjectModel>(ConstantMessage.Success, subjects, totalOrigin,
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
                var subject = await _service.GetById(id);

                return subject switch
                {
                    null => Ok(new ItemResponse<SubjectModel>(ConstantMessage.NotFound, null)),
                    not null => Ok(new ItemResponse<SubjectModel>(ConstantMessage.Success, subject))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpGet("get-all-subject-by-categoryId/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            try
            {
                if (categoryId == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var subject = await _service.GetByCategoryId(categoryId);

                return subject switch
                {
                    null => Ok(new ItemListResponse<SubjectModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<SubjectModel>(ConstantMessage.Success, subject))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllUserSearch(PaginatedRequest<SubjectSearchRequest> paginatedRequest)
        {
            try
            {
                var subject = _mapper.Map<SubjectModel>(paginatedRequest.Result);
                var subjects = await _service.Search(subject, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return subjects.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<SubjectModel>(ConstantMessage.NotFound, subjects.Item1, subjects.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<SubjectModel>(ConstantMessage.Success, subjects.Item1, subjects.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(SubjectRequest request)
        {
            try
            {
                bool isSuccess = await _service.Add(_mapper.Map<SubjectModel>(request));
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
                    var isSession = await _service.Delete(id);

                    return isSession switch
                    {
                        true => Ok(new BaseResponse(isSession, ConstantMessage.Success)),
                        _ => Ok(new BaseResponse(isSession, ConstantMessage.Fail))
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
        public async Task<IActionResult> Update(SubjectRequest request)
        {
            try
            {
                bool isSuccess = await _service.Update(_mapper.Map<SubjectModel>(request));
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
