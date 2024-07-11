using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Base
{
    public abstract class BaseService
    {

    }
    public abstract class BaseService<TEntity> : BaseService
        where TEntity : BaseEntity
    {
        protected readonly IMapper _mapper;

        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository __userRepository;

        private readonly IBaseRepository<TEntity> __repository;

        protected BaseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            __repository = _unitOfWork.GetRepositoryByEntity<TEntity>();
            __userRepository = _unitOfWork.UserRepository;
        }

        protected BaseService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : this(mapper, unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TEntity> SetBaseEntityToCreateFunc(TEntity entity)
        {

            var user = await GetUserInfo();
            if (user != null)
            {
                entity.Id = Guid.NewGuid();
                entity.CreatedBy = user.Email;
                entity.CreatedDate = DateTime.Now;
                entity.LastUpdatedBy = user.Email;
                entity.LastUpdatedDate = entity.CreatedDate;
                entity.IsDeleted = false;
            } else
            {
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
            }

            return entity;
        }
        public async Task<TEntity> SetBaseEntityToCreateFuncMany(TEntity entity)
        {

            var user = await GetUserInfo();
            if (user != null)
            {
                entity.CreatedBy = user.Email;
                entity.CreatedDate = DateTime.Now;
                entity.LastUpdatedBy = user.Email;
                entity.LastUpdatedDate = entity.CreatedDate;
                entity.IsDeleted = false;
            }
            else
            {
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
            }

            return entity;
        }

        public async Task<TEntity> SetBaseEntityToUpdateFunc(TEntity entity)
        {

            var user = await GetUserInfo();
            if (user != null)
            {
                entity.LastUpdatedBy = user.Email;
                entity.LastUpdatedDate = DateTime.Now;
            }

            return entity;
        }

        public async Task<User> GetUserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token == "null")
                {
                    return null;
                }
                if (!string.IsNullOrEmpty(token))
                {
                    var userFromToken = GetUserEmailWithUserenameFromToken(token);
                    if (userFromToken.Item1 != null && userFromToken.Item2 != null)
                    {
                        var user = await __userRepository.FindUsernameOrEmail(new User
                        {
                            Email = userFromToken.Item1,
                            Username = userFromToken.Item2
                        });

                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }

            return null;

        }

        public Task<long> GetTotalCount()
        {
            return __repository.GetTotalCount();
        }

        private (string, string) GetUserEmailWithUserenameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email"); // Hoặc bất kỳ claim nào chứa Email
            var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub"); // Hoặc bất kỳ claim nào chứa Email
            return (emailClaim?.Value, usernameClaim?.Value);
        }
    }
}
