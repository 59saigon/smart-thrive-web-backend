using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.API.ResponseModel;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using SWD.SmartThrive.Services.Services.Service;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAll();

                return roles switch
                {
                    null => Ok(new ItemListResponse<RoleModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<RoleModel>(ConstantMessage.Success, roles))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-role-name")]
        public async Task<IActionResult> GetRoleByEmail(string roleName)
        {
            try
            {
                var roleModel = await _roleService.GetRoleByName(roleName);

                return roleModel switch
                {
                    null => Ok(new ItemResponse<RoleModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<RoleModel>(ConstantMessage.Success, roleModel))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }
    }
}
