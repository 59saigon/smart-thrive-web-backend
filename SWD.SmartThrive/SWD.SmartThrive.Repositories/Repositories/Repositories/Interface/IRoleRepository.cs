using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Base;

namespace SWD.SmartThrive.Repositories.Repositories.Repositories.Interface
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetRoleByName(string name);
    }
}
