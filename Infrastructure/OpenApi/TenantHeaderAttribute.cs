using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.OpenApi
{
    public class TenantHeaderAttribute() : BaseHeaderAttribute(
        TenancyConstants.TenantIdName,
            "Input Your tenant name to access this API", string.Empty, true)
    {
        //public TenantHeaderAttribute(string headerName, string description, string defaultValue, bool isRequired) : base(TenancyConstants.TenantIdName,
        //    "Input Your tenant name to access this API", string.Empty, true)
        //{
        //}
    }
}
