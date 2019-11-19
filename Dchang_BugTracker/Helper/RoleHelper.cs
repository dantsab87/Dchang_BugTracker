using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dchang_BugTracker.Helper
{
    public class RoleHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName) 
        {
            return userManager.IsInRole(userId, roleName);
        }
        public ICollection<string> ListUserRoles(string userId) 
        {
            return userManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName) 
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName) 
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List) 
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UsersIn2Role(string roleName1, string rolename2)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName1) || IsUserInRole(user.Id, rolename2))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UserNotInRole(string roleName) 
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List) 
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }


        public bool IsDemoUser(string userId)
        {
            var roles = ListUserRoles(userId).FirstOrDefault() ?? "";
            return roles.Contains("Demo");
        }


    }
}