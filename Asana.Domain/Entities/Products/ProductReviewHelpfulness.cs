using Asana.Domain.Entities.Common;


namespace Asana.Domain.Entities.Products
{
    public class ProductReviewHelpfulness : BaseEntity , ISoftDeletable
    {
        #region Properties

        public bool WasHelpful { get; set; }

        public bool IsDelete { get; set; }

        public long ProductReviewId { get; set; }

        #endregion


        #region Relations

        public ProductReview Review { get; set; }

        #endregion

    }
}
