using Asana.Domain.Entities.Common;

namespace Asana.Domain.Entities.Products
{
    public class Color : BaseEntity, ISoftDeletable
    {

        #region Properties

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Code { get; set; }

        public bool IsDelete { get; set; }

        #endregion
    }
}
