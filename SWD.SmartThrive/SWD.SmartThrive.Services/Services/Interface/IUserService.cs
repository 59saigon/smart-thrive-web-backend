﻿using SWD.SmartThrive.Services.Model;
using System.IdentityModel.Tokens.Jwt;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IUserService
    {
        public Task<bool> AddUser(UserModel user);

        public Task<bool> UpdateUser(UserModel user);

        public Task<bool> DeleteUser(Guid id);

        public Task<UserModel> GetUser(Guid id);

        public Task<List<UserModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<List<UserModel>?> GetAll();

        public Task<(List<UserModel>?, long)> GetAllUserSearch(UserModel userModel, int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<long> GetTotalCount();

        Task<UserModel> Login(AuthModel authModel);

        Task<UserModel> Register(UserModel userModel);

        public JwtSecurityToken CreateToken(UserModel userModel);

        public Task<UserModel> GetUserByEmailOrUsername(UserModel userModel);

    }
}
