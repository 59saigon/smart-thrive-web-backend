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
    public class PackageXCourseController : Controller
    {
        private readonly IPackageXCourseService _service;
        private readonly IMapper _mapper;

        public PackageXCourseController(IPackageXCourseService service, IMapper mapper) {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("get-all-by-id-package/{id}")]
        public async Task<IActionResult> GetAllById(Guid id)
        {
            try
            {
                if(id == Guid.Empty)
                {
                    return Ok("Must be input id package");
                }
                List<CourseXPackageModel> x = await _service.GetAllByIdPackage(id);
                return x switch
                {
                    null => Ok(new ItemListResponse<CourseXPackageModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<CourseXPackageModel>(ConstantMessage.Success, x))
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
        public async Task<IActionResult> Delete(Guid idcourse , Guid idpackage)
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
