using Asana.Domain.Entities.Addresses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asana.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        [StringLength(220)]
        public string FirstName { get; set; }

        [StringLength(220)]
        public string LastName { get; set; }

        [StringLength(10)]
        public string NationalCode { get; set; }

        [StringLength(16)]
        public string CreditCardNumber { get; set; }

        public string Avatar { get; set; }


        #region Relations

        public ICollection<Address> Addresses{ get; set; }

        #endregion
    }
}
