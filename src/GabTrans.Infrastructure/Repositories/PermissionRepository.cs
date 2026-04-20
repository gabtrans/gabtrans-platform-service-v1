using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class PermissionRepository(GabTransContext context, ILogService logService) : IPermissionRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> GetPermittedRoleById(long roleId) => await _context.Permissions.AnyAsync(x => x.RoleId == roleId);

        public async Task<bool> CreateAsync(List<long> moduleActionIds, long roleId)
        {
            try
            {
                var existingActions = await _context.Permissions.Where(x => x.RoleId == roleId).ToListAsync();

                _context.Permissions.RemoveRange(existingActions);

                var permissions = moduleActionIds.ToList().Select(moduleActionId =>
                   new Permission
                   {
                       RoleId = roleId,
                       ModuleActionId = moduleActionId,
                   });

                await _context.AddRangeAsync(permissions);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(PermissionRepository), nameof(CreateAsync), ex);
            }

            return false;
        }

        public async Task<IEnumerable<PermissionModel>> GetByRoleIdAsync(long roleId)
        {
            return await (from p in _context.Permissions.AsNoTracking()
                          from r in _context.Roles.AsNoTracking().Where(x => x.Id == p.RoleId).DefaultIfEmpty()
                          from ma in _context.ModuleActions.AsNoTracking().Where(x => x.Id == p.ModuleActionId).DefaultIfEmpty()
                          from m in _context.Modules.AsNoTracking().Where(m => m.Id == ma.ModuleId).DefaultIfEmpty()
                          where p.RoleId == roleId
                          select new PermissionModel()
                          {
                              ModuleId = m.Id,
                              ModuleName = m.Name,
                              PermissionActions = new List<PermissionActions>()
                                {
                                    new()
                                    {
                                        ModuleActionId = ma.Id,
                                        Name = ma.UserAction
                                    }
                                }
                          }).ToListAsync();
        }


        public async Task<IEnumerable<ModuleModel>> GetAllAsync()
        {
            return await (from ma in _context.ModuleActions.AsNoTracking()
                          from m in _context.Modules.Where(x => x.Id == ma.ModuleId).AsNoTracking().DefaultIfEmpty()
                          select new ModuleModel()
                          {
                              ModuleId = m.Id,
                              ModuleName = m.Name,
                              Actions = new List<Actions>()
                                {
                                    new()
                                    {
                                        ModuleActionId = ma.Id,
                                        ModuleActionName = ma.UserAction,
                                        Checked = false
                                    }
                                }
                          }).ToListAsync();
        }

        public async Task<IEnumerable<PermittedRole>> GetAllPermittedRolesAsync()
        {
            return await _context.Permissions.Include(y => y.Role).Select(x =>
            new PermittedRole
            {
                Id = x.RoleId,
                Role = x.Role.Name,
                Status = x.Role.Status,
            }).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<PermissionModel>> GetByUserIdAsync(long userId)
        {
            return await (from p in _context.Permissions.AsNoTracking()
                          join moduleAction in _context.ModuleActions on p.ModuleActionId equals moduleAction.Id into ModuleActions
                          from ma in ModuleActions.DefaultIfEmpty()
                          join module in _context.Modules on ma.ModuleId equals module.Id into Modules
                          from m in Modules.DefaultIfEmpty()
                          join userRole in _context.UserRoles on p.RoleId equals userRole.RoleId into UserRoles
                          from ur in UserRoles.DefaultIfEmpty()
                          join user in _context.Users on ur.UserId equals user.Id into Users
                          from u in Users.DefaultIfEmpty()
                          join role in _context.Roles on p.RoleId equals role.Id into Roles
                          from r in Roles.DefaultIfEmpty()

                          where ur.UserId == u.Id && ur.UserId == userId

                          select new PermissionModel
                          {
                              ModuleId = m.Id,
                              ModuleName = m.Name,
                              RoleId = r.Id,
                              Role = r.Name,
                              PermissionActions = new List<PermissionActions>
                                {
                                    new()
                                    {
                                        ModuleActionId = ma.Id,
                                        Name = ma.UserAction
                                    }
                                }
                          }).ToListAsync();
        }
    }
}
