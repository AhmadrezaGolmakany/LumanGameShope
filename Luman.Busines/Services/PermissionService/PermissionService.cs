using Luman.DataLayer.Context;
using Luman.DataLayer.EntityModel.Permitions;
using Luman.DataLayer.EntityModel.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luman.Busines.Services.PermissionService
{
    public class PermissionService : IPermissionService
    {

        private readonly LumanContext _context;

        public PermissionService(LumanContext context)
        {
            _context = context;
        }

        public void AddRoleUser(int RoleId, int userId)
        {

            _context.userRoles.Add(new UserRole
            {
                RoleId = RoleId,
                UserId = userId
            });


            _context.SaveChanges();
        }

        public bool CheckPermission(int permissionId, string? username)
        {
            var userId = _context.users.Single(u => u.UserName == username).UserId;


            var userRoles = _context.userRoles.Where(u => u.UserId == userId)
                .Select(r => r.RoleId).ToList();

            if (!userRoles.Any())
                return false;

            List<int> RolePermission = _context.rolePermissions
                .Where(r => r.PermissionID == permissionId)
                .Select(r => r.RoleId).ToList();

            return RolePermission.Any(p => userRoles.Contains(p));
        }

        public List<Permition> GetAllPermission()
        {
            return _context.permitions.ToList();
        }

        public List<Role> GetRole()
        {
            return _context.roles.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.roles.Find(roleId);
        }


    }
}
