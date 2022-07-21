using Microsoft.EntityFrameworkCore;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.DatabaseAccessor.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.Web.Core.Cache;
using PH.Blog.Contract;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class AuthenticationSvc : IAuthenticationSvc
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IRepository<Privilege> _privilegeRepo;
        private readonly ICacheProvide _cacheProvide;

        public AuthenticationSvc
            (
            IRepository<User> userRepo,
            IRepository<Role> roleRepo,
            IRepository<Privilege> privilegeRepo,
            ICacheProvide cacheProvide
            )
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _privilegeRepo = privilegeRepo;
            _cacheProvide = cacheProvide;
        }

        /// <summary>
        /// 通过 id 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetAsync(int userId)
        {
            return await _userRepo.FindOrDefaultAsync(userId);
        }

        /// <summary>
        /// 通过账号获取用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<User> GetByAccountAsync(string account)
        {
            return await _userRepo.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Account == account);
        }

        /// <summary>
        /// 通过 RoleCode 获取 Privilege
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<string>> GetPrivilegeByRoleCodeAsync(IEnumerable<string> roleCodes)
        {
            var rolePrivileges = await _cacheProvide.GetAsync(ConstPool.CACHEKEY_PRIVILEGE, () =>
               {
                   var rolePrivileges = _roleRepo.Include(x => x.Privileges).ToList();
                   return rolePrivileges.Select(x => new KeyValuePair<string, IEnumerable<string>>(x.Code, x.Privileges.Select(p => p.Code)));
               }, 60 * 120);

           return rolePrivileges.Where(x => roleCodes.Contains(x.Key)).SelectMany(x => x.Value).Distinct().ToList();
        }
    }
}
