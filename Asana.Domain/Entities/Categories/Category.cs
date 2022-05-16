using Asana.Domain.Entities.Common;
using Asana.Domain.Entities.Media;
using Asana.Domain.Entities.Products;
using System.Collections.Generic;

namespace Asana.Domain.Entities.Categories
{
    public class Category : BaseEntity, ISoftDeletable
    {
        #region properties
         
        public string Name { get; set; }

        public string Description { get; set; }

        public long? ParentId { get; set; }

        public bool IsDelete { get; set; }

        public bool SizeVariety { get; set; }

        public bool ColorVariety { get; set; }

        public long MediaFileId { get; set; }

        #endregion

        #region Relations

        public Category ParentCategory { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Product> Products { get; set; }

        public MediaFile MediaFile { get; set; }

        #endregion
    }
}
