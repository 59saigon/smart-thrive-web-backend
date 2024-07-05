using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.ResponseModel;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.API.Tool.Mapping;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/coursexpackage")]
    [ApiController]
    [Authorize]
    public class CourseXPackageController : Controller
    {
        private readonly ICourseXPackageService _service;
        private readonly IMapper _mapper;

        public CourseXPackageController(ICourseXPackageService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("get-all-by-package-id/{packageId}")]
        public async Task<IActionResult> GetAllById(Guid packageId)
        {
            try
            {
                if (packageId == Guid.Empty)
                {
                    return Ok("Must be input package id");
                }
                List<CourseXPackageModel> courseXPackages = await _service.GetAllByPackageId(packageId);
                return courseXPackages switch
                {
                    null => Ok(new ItemListResponse<CourseXPackageModel>(ConstantMessage.Fail)),
                    not null => Ok(new ItemListResponse<CourseXPackageModel>(ConstantMessage.Success, courseXPackages))
                };

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddCourse(PackageXCourseRequest course)
        {
            try
            {
                var isCourse = await _service.Add(_mapper.Map<CourseXPackageModel>(course));

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
        public async Task<IActionResult> Delete(Guid idcourse, Guid idpackage)
        {
            try
            {
                if (idpackage != Guid.Empty && idcourse != Guid.Empty)
                {
                    var isCourse = await _service.Delete(idcourse, idpackage);

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
    }
}
