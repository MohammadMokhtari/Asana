using Asana.Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace Asana.Domain.Entities.Products
{
    public class ProductReview : BaseEntity, ISoftDeletable
    {
        #region Properties

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public sbyte Rating { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }

        public bool IsDelete { get; set; }

        public long ProductId { get; set; }

        public Guid UserId { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }

        public ICollection<ProductReviewHelpfulness> ReviewHelpfulnesses { get; set; }

        #endregion

    }
}
