using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users
{
    public class ChangeUserStatusRequest
    {
        public string UserId { get; set; }
        public bool Activation { get; set; }
    }
}
