using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IPermissionRepository
    {
        Task<bool> GetPermittedRoleById(long roleId);
        Task<bool> CreateAsync(List<long> moduleActionIds, long roleId);
        Task<IEnumerable<PermissionModel>> GetByRoleIdAsync(long roleId);
        Task<IEnumerable<PermissionModel>> GetByUserIdAsync(long userId);
        Task<IEnumerable<ModuleModel>> GetAllAsync();
        Task<IEnumerable<PermittedRole>> GetAllPermittedRolesAsync();
    }
}
