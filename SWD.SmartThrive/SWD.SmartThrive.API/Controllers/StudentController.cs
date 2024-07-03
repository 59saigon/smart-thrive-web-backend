using AutoMapper;
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
using SWD.SmartThrive.Services.Services.Service;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/student")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentController(IMapper mapper, IStudentService studentService)
        {
            _mapper = mapper;
            _studentService = studentService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var students = await _studentService.GetAll();

                return students switch
                {
                    null => Ok(new ItemListResponse<StudentModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<StudentModel>(ConstantMessage.Success, students))
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
                var providers = await _studentService.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _studentService.GetTotalCount();

                return providers switch
                {
                    null => Ok(new PaginatedListResponse<StudentModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<StudentModel>(ConstantMessage.Success, providers, totalOrigin,
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
                var StudentModel = await _studentService.GetById(id);

                return StudentModel switch
                {
                    null => Ok(new ItemResponse<StudentModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<StudentModel>(ConstantMessage.Success, StudentModel))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpGet("get-by-userId")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var StudentModel = await _studentService.GetStudentsByUserId(userId);

                return StudentModel switch
                {
                    null => Ok("No student with given userId"),
                    not null => Ok(StudentModel)
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllUserSearch(PaginatedRequest<StudentSearchRequest> paginatedRequest)
        {
            try
            {
                var student = _mapper.Map<StudentModel>(paginatedRequest.Result);
                var students = await _studentService.Search(student, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return students.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<StudentModel>(ConstantMessage.NotFound, students.Item1, students.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<StudentModel>(ConstantMessage.Success, students.Item1, students.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(StudentRequest request)
        {
            try
            {
                bool isSuccess = await _studentService.Add(_mapper.Map<StudentModel>(request));
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
                    var isSession = await _studentService.Delete(id);

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
        public async Task<IActionResult> Update(StudentRequest request)
        {
            try
            {
                bool isSuccess = await _studentService.Update(_mapper.Map<StudentModel>(request));
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
