using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Application.Common.Models
{
    public class ProcessImageModel
    {
        public string FileName { get; set; }

        public string Type { get; set; }

        public Stream Content { get; set; }
    }
}
