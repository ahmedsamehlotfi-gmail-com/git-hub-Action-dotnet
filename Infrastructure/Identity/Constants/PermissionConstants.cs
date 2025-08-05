using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Constants
{
    public static class PermissionConstants
    {
        public static class SchoolAction
        {
            public const string view = nameof(view);
            public const string Create = nameof(Create);
            public const string Update = nameof(Update);
            public const string Delete = nameof(Delete);
            public const string UpgradeSubscription = nameof(UpgradeSubscription);
        }

        public static class SchoolFeature
        {
            public const string Tenants = nameof(Tenants);
            public const string Users = nameof(Users);
            public const string UserRoles = nameof(UserRoles);
            public const string Roles = nameof(Roles);
            public const string RolesClaims = nameof(RolesClaims);
            public const string Schools = nameof(Schools);

        }

        public record SchoolPermission(string Description, string Action, string Feature, bool IsBasic = false, bool IsRoot = false)
        {
            public string Name => NameFor(Action, Feature);
            public static string NameFor(string action, string feature) => $"Permission.{feature}.{action}";
        }

        public static class SchoolPermissions
        {
            public static readonly SchoolPermission[] _allPermisssion =
            [
            new SchoolPermission("View Users" , SchoolAction.view , SchoolFeature.Users),
            new SchoolPermission("Create Users" , SchoolAction.Create , SchoolFeature.Users),
            new SchoolPermission("Update Users" , SchoolAction.Update , SchoolFeature.Users),
            new SchoolPermission("Delete Users" , SchoolAction.Delete , SchoolFeature.Users),

            new SchoolPermission("View User Roles" , SchoolAction.view , SchoolFeature.UserRoles),
            new SchoolPermission("Update User Roles" , SchoolAction.Update , SchoolFeature.UserRoles),

            new SchoolPermission("View Roles" , SchoolAction.view , SchoolFeature.Roles),
            new SchoolPermission("Create Roles" , SchoolAction.Create , SchoolFeature.Roles),
            new SchoolPermission("Update Roles" , SchoolAction.Update , SchoolFeature.Roles),
            new SchoolPermission("Delete Roles" , SchoolAction.Delete , SchoolFeature.Roles),

            new SchoolPermission("View Role Claims/Permissions" , SchoolAction.view , SchoolFeature.RolesClaims),
            new SchoolPermission("Update Role Claims/Permissions" , SchoolAction.Update , SchoolFeature.RolesClaims),

            new SchoolPermission("View Schools" , SchoolAction.view , SchoolFeature.Schools , IsBasic:true),
            new SchoolPermission("Create Schools" , SchoolAction.Create , SchoolFeature.Schools),
            new SchoolPermission("Update Schools" , SchoolAction.Update , SchoolFeature.Schools),
            new SchoolPermission("Delete Schools" , SchoolAction.Delete , SchoolFeature.Schools),


            new SchoolPermission("View Tenants" , SchoolAction.view , SchoolFeature.Tenants , IsRoot:true),
            new SchoolPermission("Create Tenants" , SchoolAction.Create , SchoolFeature.Tenants ,IsRoot:true),
            new SchoolPermission("Update Tenants" , SchoolAction.Update , SchoolFeature.Tenants , IsRoot:true),
            new SchoolPermission("Upgrade Tenants Subscription" , SchoolAction.UpgradeSubscription , SchoolFeature.Tenants , IsRoot:true),
        ];

            public static IReadOnlyList<SchoolPermission> All { get; } = new ReadOnlyCollection<SchoolPermission>(_allPermisssion);
            public static IReadOnlyList<SchoolPermission> Root { get; } = new ReadOnlyCollection<SchoolPermission>(_allPermisssion.Where(p => p.IsRoot).ToArray());
            public static IReadOnlyList<SchoolPermission> Admin { get; } = new ReadOnlyCollection<SchoolPermission>(_allPermisssion.Where(p => !p.IsRoot).ToArray());
            public static IReadOnlyList<SchoolPermission> Basic { get; } = new ReadOnlyCollection<SchoolPermission>(_allPermisssion.Where(p => p.IsBasic).ToArray());
        }
    }
}
