using Asana.Domain.Entities.Common;
using System.Collections.Generic;

namespace Asana.Domain.Entities.Products
{
    public class ProductWarranti : BaseEntity , ISoftDeletable
    {
        #region Properties

        public string WarrantiName { get; set; }

        public string Description { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
