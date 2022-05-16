using Asana.Domain.Entities.Categories;
using Asana.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asana.Domain.Entities.Media
{
    public class MediaFile : BaseEntity , IMediaFile ,ISoftDeletable
    {
        #region Properties

        public string MediaName { get; set; }

        public string Alt { get; set; }

        public string FolderPath { get; set; }

        public bool IsDelete { get; set; }

        public int MediaTypeId { get; set; }
                
        [NotMapped]
        public MediaType Type
        {
            get => (MediaType)MediaTypeId;
            set => MediaTypeId = (int)value;
        }

        #endregion

    }
}
