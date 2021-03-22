using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2rayGeodataAutoupdater.Data
{
    public class SourceStatus
    {
        public string Url { get; set; }
        public bool CanAccess { get; set; }
        public DateTime LastModify { get; set; }
    }
}
