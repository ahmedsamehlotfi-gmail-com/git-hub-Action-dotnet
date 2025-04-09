using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Model
{
    public class CreateTenantRequest
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string AdminEmail { get; set; }
        public DateTime ValidUpTo { get; set; }
        public bool IsActive { get; set; }
    }
}
