using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles
{
    public class UpdateRolePermissions
    {
        public string RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
