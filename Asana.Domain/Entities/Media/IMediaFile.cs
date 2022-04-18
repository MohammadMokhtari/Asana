using Asana.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asana.Domain.Entities.Media
{
    public interface IMediaFile 
    {

        public string MediaName { get; set; }

        public string Alt { get; set; }

        public string FolderPath { get; set; }

        public bool IsDelete { get; set; }

        public int MediaTypeId { get; set; }

        public MediaType Type { get; set; }

    }
}
