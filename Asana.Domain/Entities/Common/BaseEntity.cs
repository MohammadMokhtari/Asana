using System;

namespace Asana.Domain.Entities.Common
{
    public class BaseEntity
    {
        #region Properties

        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        #endregion
    }
}
