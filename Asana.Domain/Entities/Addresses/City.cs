using Asana.Domain.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asana.Domain.Entities.Addresses
{
    public class City : BaseEntity , ISoftDeletable
    {
        #region Properties

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string ProvinceName { get; set; }

        public bool IsDelete { get; set; }

        #endregion


        #region Relations

        public Province State { get; set; }

        public ICollection<Address> Addresses { get; set; }

        #endregion
    }
}
