using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FileObject
    {
        public string Extension { get; set; }
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }
}
