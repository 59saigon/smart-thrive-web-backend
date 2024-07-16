using SWD.SmartThrive.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Interface
{
    public interface IRoleService
    {
        public Task<RoleModel> GetRoleByName(string roleName);
        Task<List<RoleModel>> GetAll();
    }
}
