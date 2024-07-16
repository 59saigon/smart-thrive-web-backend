using AutoMapper;
using SWD.SmartThrive.Repositories.Data.Entities;
using SWD.SmartThrive.Repositories.Repositories.Repositories.Interface;
using SWD.SmartThrive.Repositories.Repositories.UnitOfWork.Interface;
using SWD.SmartThrive.Services.Base;
using SWD.SmartThrive.Services.Model;
using SWD.SmartThrive.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SmartThrive.Services.Services.Service
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
            _roleRepository = unitOfWork.RoleRepository;
        }

        public async Task<List<RoleModel>> GetAll()
        {
            var roles = await _roleRepository.GetAll();

            if (!roles.Any())
            {
                return null;
            }

            return _mapper.Map<List<RoleModel>>(roles);
        }

        public async Task<RoleModel> GetRoleByName(string roleName)
        {
            if (roleName == null || roleName != "Buyer" || roleName != "Staff" || roleName != "Admin" || roleName != "Provider")
            {
                roleName = "Buyer";
            }

            Role role = await _roleRepository.GetRoleByName(roleName);

            return _mapper.Map<RoleModel>(role);
        }
    }
}
