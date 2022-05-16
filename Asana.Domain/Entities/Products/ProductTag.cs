using Asana.Domain.Entities.Common;
using System.Collections.Generic;

namespace Asana.Domain.Entities.Products
{
    public class ProductTag : BaseEntity, ISoftDeletable
    {

        #region Properties

        public string Name { get; set; }

        public bool IsDelete { get; set; }
        #endregion

        #region Relations

        public ICollection<Product> Products { get; set; }

        #endregion

    }
}
