using Asana.Domain.Entities.Common;

namespace Asana.Domain.Entities.Products
{
    public class SpecializedProductReview : BaseEntity, ISoftDeletable
    {
        #region Properties

        public string SpecializedReviewText { get; set; }

        public long ProductId { get; set; }

        public bool IsDelete { get; set; }
        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion

    }
}
