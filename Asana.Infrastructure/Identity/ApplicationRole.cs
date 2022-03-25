using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Asana.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        [StringLength(700)]
        public string Description { get; set; }

        public bool IsSystemRole { get; set; }
    }
}
