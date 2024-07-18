using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD.SmartThrive.API.RequestModel;
using SWD.SmartThrive.API.ResponseModel;
using SWD.SmartThrive.API.SearchRequest;
using SWD.SmartThrive.API.Tool.Constant;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using SWD.SmartThrive.Services.Services.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SWD.SmartThrive.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IRoleService roleService, IMapper mapper)
        {
            _service = service;
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _service.GetAll();

                return users switch
                {
                    null => Ok(new ItemListResponse<UserModel>(ConstantMessage.Fail, null)),
                    not null => Ok(new ItemListResponse<UserModel>(ConstantMessage.Success, users))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllUserSearch(PaginatedRequest<UserSearchRequest> paginatedRequest)
        {
            try
            {
                var user = _mapper.Map<UserModel>(paginatedRequest.Result);
                var users = await _service.Search(user, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);

                return users.Item1 switch
                {
                    null => Ok(new PaginatedListResponse<UserModel>(ConstantMessage.NotFound, users.Item1, users.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder)),
                    not null => Ok(new PaginatedListResponse<UserModel>(ConstantMessage.Success, users.Item1, users.Item2, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpPost("get-all-pagination")]
        public async Task<IActionResult> GetAllUser(PaginatedRequest paginatedRequest)
        {
            try
            {
                var users = await _service.GetAllPagination(paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField, paginatedRequest.SortOrder.Value);
                long totalOrigin = await _service.GetTotalCount();
                return users switch
                {
                    null => Ok(new PaginatedListResponse<UserModel>(ConstantMessage.NotFound)),
                    not null => Ok(new PaginatedListResponse<UserModel>(ConstantMessage.Success, users, totalOrigin, paginatedRequest.PageNumber, paginatedRequest.PageSize, paginatedRequest.SortField))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Id is empty");
                }
                var userModel = await _service.GetById(id);

                return userModel switch
                {
                    null => Ok(new ItemResponse<UserModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<UserModel>(ConstantMessage.Success, userModel))
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            };
        }

        [AllowAnonymous]
        [HttpGet("get-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var userModel = await _service.GetUserByEmail(new UserModel { Email = email });

                return userModel switch
                {
                    null => Ok(new ItemResponse<UserModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<UserModel>(ConstantMessage.Success, userModel))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser(UserRequest user)
        {
            try
            {
                var isUser = await _service.Add(_mapper.Map<UserModel>(user));

                return isUser switch
                {
                    true => Ok(new BaseResponse(isUser, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isUser, ConstantMessage.Fail))
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
        public async Task<IActionResult> Update(UserRequest user)
        {
            try
            {
                var userModel = _mapper.Map<UserModel>(user);

                var isUser = await _service.Update(userModel);

                return isUser switch
                {
                    true => Ok(new BaseResponse(isUser, ConstantMessage.Success)),
                    _ => Ok(new BaseResponse(isUser, ConstantMessage.Fail))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel authModel)
        {
            try
            {
                var userModel = await _service.Login(authModel);

                if (userModel == null)
                {
                    return Ok(new LoginResponse<UserModel>(null, null, null, ConstantMessage.Fail));
                }

                JwtSecurityToken token = _service.CreateToken(userModel);

                return Ok(new LoginResponse<UserModel>(ConstantMessage.Success, userModel, new JwtSecurityTokenHandler().WriteToken(token)
                , token.ValidTo.ToString()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(OtpEmailRequest otpEmailRequest)
        {
            try
            {
                var otp = _service.GenerateOTP();
                _service.SendEmail(otpEmailRequest.Email.ToLower(), otp);
                _service.StoreOtp(otpEmailRequest.Email.ToLower(), otp);
                return Ok(new { message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP(OtpEmailRequest otpEmailRequest)
        {
            try
            {
                if (_service.VerifyOtp(otpEmailRequest.Email.ToLower(), otpEmailRequest.Otp))
                {
                    return Ok(new { valid = true });
                }
                else
                {
                    return Ok(new { valid = false });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.NewPassword))
                {
                    return Ok(new BaseResponse(false, "Email, OTP, and new password are required."));
                }


                // get by email
                var userModel = await _service.GetUserByEmail(new UserModel { Email = request.Email });

                if (userModel != null)
                {
                    if (string.IsNullOrEmpty(userModel.Password))
                    {
                        return Ok(new BaseResponse(false, "This account just only login by google!"));
                    }
                    else
                    {
                        userModel.Password = request.NewPassword;
                        var isUser = await _service.UpdatePassword(userModel);

                        return isUser switch
                        {
                            true => Ok(new BaseResponse(isUser, ConstantMessage.Success)),
                            _ => Ok(new BaseResponse(isUser, ConstantMessage.Fail))
                        };
                    }
                }
                else
                {
                    return Ok(new BaseResponse(false, "User not found."));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("login-with-another")]
        public async Task<IActionResult> LoginWithAnother([FromBody] UserVerifyEmail userVerifyEmail)
        {
            try
            {
                var payload = await VerifyGoogleToken(userVerifyEmail.GoogleToken);
                if (payload == null)
                {
                    return Ok(new LoginResponse<UserModel>(null, null, null, "Invalid Google Token"));
                }
                UserModel user = null;
                user = await _service.GetUserByEmail(new UserModel { Email = payload.Email });
                if (user == null)
                {
                    // register 
                    RoleModel roleModel = await _roleService.GetRoleByName("Buyer");

                    // Lấy base64 của ảnh
                    string base64Image = await GetBase64ImageFromUrl(payload.Picture);

                    UserModel userModel = new UserModel
                    {
                        Username = payload.Subject,
                        Email = payload.Email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        FullName = payload.Name,
                        RoleId = roleModel.Id,
                        Picture = base64Image
                    };

                    user = await _service.Register(userModel);
                }

                // Create JWT token
                JwtSecurityToken token = _service.CreateToken(user);

                return Ok(new LoginResponse<UserModel>(ConstantMessage.Success, user, new JwtSecurityTokenHandler().WriteToken(token)
                , token.ValidTo.ToString()));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private async Task<string> GetBase64ImageFromUrl(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return Convert.ToBase64String(imageBytes);
            }
        }


        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "311399879185-vic40gludgaeulfo790m0h48h1cvul7u.apps.googleusercontent.com" }
            };

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload;
        }


        [AllowAnonymous]
        // POST api/<AuthController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            try
            {
                UserModel _userModel = await _service.GetUserByEmailOrUsername(_mapper.Map<UserModel>(userRequest));

                if (_userModel != null)
                {
                    return Ok(new ItemResponse<UserModel>(ConstantMessage.Duplicate));
                }

                RoleModel roleModel = await _roleService.GetRoleByName(userRequest.RoleName);

                UserModel userModelMapping = _mapper.Map<UserModel>(userRequest);

                userModelMapping.RoleId = roleModel.Id;
                userModelMapping.Password = userRequest.Password;

                UserModel userModel = await _service.Register(userModelMapping);

                return userModel switch
                {
                    null => Ok(new ItemResponse<UserModel>(ConstantMessage.NotFound)),
                    not null => Ok(new ItemResponse<UserModel>(ConstantMessage.Success, userModel))
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
