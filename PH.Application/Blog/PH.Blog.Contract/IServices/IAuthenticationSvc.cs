using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IAuthenticationSvc
    {
        /// <summary>
        /// 通过账号获取用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        Task<User> GetByAccountAsync(string account);

        /// <summary>
        /// 通过 id 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetAsync(int userId);

        /// <summary>
        /// 获取角色所拥有的权限
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetPrivilegeByRoleCodeAsync(IEnumerable<string> roleCodes);
    }
}
