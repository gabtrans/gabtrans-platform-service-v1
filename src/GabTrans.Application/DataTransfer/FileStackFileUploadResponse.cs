using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FileStackFileUploadResponse
    {
        public string url { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string filename { get; set; }
        public string key { get; set; }
    }
}
