﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.Services.Services.Interface;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.API.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using SWD.SmartThrive.Services.Services.Service;
using SWD.SmartThrive.API.SearchRequest;
using SWD.SmartThrive.Repositories.Data.Entities;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/course")]
    [ApiController]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        private readonly IMapper _mapper;

        public CourseController(ICourseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = await _service.GetAll();

                return courses switch
                {
                    null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Success, courses))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("get-all-pending-status")]
        public async Task<IActionResult> GetAllPendingStatus()
        {
            try
            {
                var courses = await _service.GetAllPendingStatus();

                return courses switch
                {
                    null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Success, courses))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var courseModel = await _service.GetById(id);

                return courseModel switch
                {
                    null => Ok(new ItemResponse<CourseModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<CourseModel>(ConstantMessage.Success, courseModel))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpGet("get-all-by-provider-id/{providerId}")]
        public async Task<IActionResult> GetAllByProviderId(Guid providerId)
        {
            try
            {
                if (providerId == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var courseModels = await _service.GetAllByProviderId(providerId);

                return courseModels switch
                {
                    null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Success, courseModels))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourse(CourseRequest course)
        {
            try
            {
                var isCourse = await _service.Add(_mapper.Map<CourseModel>(course));

                return isCourse switch
                {
                    true => Ok(new BaseResponse(isCourse, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isCourse, ConstantMessage.Fail))
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
                    var isCourse = await _service.Delete(id);

                    return isCourse switch
                    {
                        true => Ok(new BaseResponse(isCourse, ConstantMessage.Success)),
                        _ => Ok(new BaseResponse(isCourse, ConstantMessage.Fail))
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
        public async Task<IActionResult> Update(CourseRequest course)
        {
            try
            {
                var courseModel = _mapper.Map<CourseModel>(course);

                var isCourse = await _service.Update(courseModel);

                return isCourse switch
                {
                    true => Ok(new BaseResponse(isCourse, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isCourse, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllCourseSearch(PaginatedRequest<CourseSearchRequest> paginatedRequest)
        {
            try
            {
                var course = _mapper.Map<CourseModel>(paginatedRequest.Result);
                var courses = await _service.Search(course, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return courses.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.NotFound, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.Success, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-pagination")]
        public async Task<IActionResult> GetAllCoures(PaginatedRequest paginatedRequest)
        {
            try
            {
                var courses = await _service.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _service.GetTotalCount();
                return courses switch
                {
                    null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.Success, courses, totalOrigin, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-pagination-by-provider-id")]
        public async Task<IActionResult> GetAllPaginationByProviderId(Guid providerId, [FromBody] PaginatedRequest paginatedRequest)
        {
            try
            {
                var courses = await _service.GetAllPaginationByProviderId(providerId, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return courses.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.NotFound, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.Success, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-pagination-by-list-id")]
        public async Task<IActionResult> GetAllCoures(PaginatedRequest<List<Guid>> paginatedRequest)
        {
            try
            {
                var courses = await _service.GetAllPaginatiomByListId(paginatedRequest.Result, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return courses.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.NotFound, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField)),
                    not null => Ok(new PaginatedListResponse<CourseModel>(ConstantMessage.Success, courses.Item1, courses.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-except-list-id-with-actived")]
        public async Task<IActionResult> GetAllExceptListId(PaginatedRequest<List<Guid>> paginatedRequest)
        {
            try
            {
                var courses = await _service.GetAllExceptListId(paginatedRequest.Result);

                return courses switch
                {
                    null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CourseModel>(ConstantMessage.Success, courses))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }
    }
}
