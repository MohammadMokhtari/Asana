using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        Guid GuidUserId { get; }
    }
}
