using Asana.Domain.Entities.Common;
using Asana.Domain.Entities.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asana.Domain.Entities.Products
{
    public class ProductMediaFile : BaseEntity, IMediaFile, ISoftDeletable
    {
        #region Properties

        public string MediaName { get; set; }

        public string Alt { get; set; }

        public string FolderPath { get; set; }

        public bool IsDelete { get; set; }

        public int MediaTypeId { get; set; }

        public long ProductId { get; set; }

        [NotMapped]
        public MediaType Type
        {
            get => (MediaType)MediaTypeId;
            set => MediaTypeId = (int)value;
        }

        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion
    }
}
