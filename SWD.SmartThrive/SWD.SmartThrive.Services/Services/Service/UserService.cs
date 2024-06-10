﻿using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Repository;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWD.SmartThrive.Services.Services.Service
{
    

    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _repository;

        private readonly IConfiguration _configuration;

        private DateTime countDown = DateTime.Now.AddMinutes(30);

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(mapper, unitOfWork)
        {
            _repository = unitOfWork.UserRepository;
            _configuration = configuration;
        }

        public async Task<bool> AddUser(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            user.Id = Guid.NewGuid();
            return await _repository.AddUser(user);
        }

        public async Task<bool> UpdateUser(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            return await _repository.UpdateUser(user);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _repository.GetUser(id);
            if (user != null)
            {
                user.IsDeleted = true;
                var isUser = await _repository.UpdateUser(user);

                if (isUser)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            var users = await _repository.GetAllUser();

            if (users == null)
            {
                return null;
            }

            return _mapper.Map<List<UserModel>>(users);
        }

        public async Task<UserModel> GetUser(Guid id)
        {
            var user = await _repository.GetUser(id);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> Login(AuthModel authModel)
        {
            User userHasUsernameOrEmail = new User
            {
                Email = authModel.UsernameOrEmail,
                Username = authModel.UsernameOrEmail,
                Password = authModel.Password,
            };
            // check username or email
            User user = await _repository.FindUsernameOrEmail(userHasUsernameOrEmail);
            if (user == null)
            {
                return null;
            }

            // check password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(authModel.Password, user.Password);
            if (!isPasswordValid)
            {
                return null;
            }

            UserModel userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }

        public async Task<UserModel> Register(UserModel userModel)
        {
            userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            bool isUser = await this.AddUser(userModel);

            if (!isUser)
            {
                return null;
            }

            return userModel;
        }

        public JwtSecurityToken CreateToken(UserModel userModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, userModel.Username),
            };
            // Conditional addition of claim based on function result
            if (!string.IsNullOrEmpty(userModel.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, userModel.Email));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Appsettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: countDown
                );

            return token;
        }
    }
}
