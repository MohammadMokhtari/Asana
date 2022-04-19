using Asana.Domain.Entities.Addresses;
using Asana.Domain.Entities.Media;
using Asana.Domain.Entities.Token;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int GenderId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public Gender Gender 
        {
            get => (Gender)GenderId;
            set => GenderId = (int)value;
        }

        #region Relations

        public ICollection<Address> Addresses{ get; set; }

        public UserMediaFile MediaFile { get; set; }

        public ICollection<RefreshToken> RefreshToken { get; set; }

        #endregion
    }
}
