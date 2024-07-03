using SWD.SmartThrive.Services.Model;
using System.IdentityModel.Tokens.Jwt;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IUserService
    {
        Task<bool> Add(UserModel userModel);

        Task<bool> Update(UserModel userModel);

        Task<bool> Delete(Guid id);
        public Task<UserModel?> GetById(Guid id);

        public Task<List<UserModel>?> GetAllPagination(int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<List<UserModel>?> GetAll();

        public Task<(List<UserModel>?, long)> Search(UserModel userModel, int pageNumber, int pageSize, string sortField, int sortOrder);

        public Task<long> GetTotalCount();

        Task<UserModel> Login(AuthModel authModel);

        Task<UserModel> Register(UserModel userModel);

        public JwtSecurityToken CreateToken(UserModel userModel);

        public Task<UserModel> GetUserByEmailOrUsername(UserModel userModel);
        
    }
}
