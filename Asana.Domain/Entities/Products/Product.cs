using Asana.Domain.Entities.Categories;
using Asana.Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace Asana.Domain.Entities.Products
{
    public class Product : BaseEntity, ISoftDeletable
    {

        #region Properties 

        public string PersianName { get; set; }

        public string EnglishName { get; set; }

        public int ProductNumber { get; set; }

        public short Quantity { get; set; }

        public int Lenght { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Weight { get; set; }

        public bool IsDelete { get; set; }

        public decimal Price { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStartDate { get; set; }

        public DateTime? SpecialPriceEndDate { get; set; }

        public DateTime? AvailableStartDate { get; set; }

        public DateTime? AvailableEndDate { get; set; }

        public string ShortDescription { get; set; }

        public bool Pulished { get; set; }

        public long? WarrantiPeriod { get; set; }

        public long WarrantiId { get; set; }

        public long CategoryId { get; set; }

        public long? ColorId { get; set; }

        #endregion

        #region Relations

        public Category Category { get; set; }

        public ICollection<ProductMediaFile> MediaFiles { get; set; }

        public ICollection<ProductReview> Reviews { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }

        public SpecializedProductReview SpecializedReview { get; set; }

        public ProductWarranti Warranti { get; set; }

        //public ICollection<Discount> Discounts { get; set; }

        //public ICollection<ProductManufacturer> Manufacturers { get; set; }

        //public ICollection<ProductVariant> Variants { get; set; }

        public Color Color  { get; set; }

        //public ICollection<ProductSpecificationAttribute> ProductSpecificationAttributes { get; set; }


        #endregion
    }
}
