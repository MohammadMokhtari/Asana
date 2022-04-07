﻿using Asana.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Asana.Domain.Entities.Addresses
{
    public class Address : BaseEntity, ISoftDeletable
    {

        #region Properties

        [Required]
        [StringLength(1000)]
        public string AddressLine { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        public string NumberPlate { get; set; }


        public byte UnitNumber { get; set; }
        
        [Required]
        [StringLength(200)]
        public string RecipientFirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string RecipientLastName { get; set; }

        [Required]
        [StringLength(10)]
        public string RecipientNationalCode { get; set; }

        [Required]
        [StringLength(11)]
        public string RecipientPhoneNumber { get; set; }
        

        public bool IsDefault { get; set; }


        public bool IsDelete { get; set; }


        public string CityName { get; set; }
         
        public string StateName { get; set; }
        
        public Guid UserId { get; set; }

        #endregion

        #region NavigationProperties

        public City City { get; set; }

        public State State { get; set; }

        #endregion

    }
}