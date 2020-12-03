using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.IM
{
    public class IMUploadResponse<T>
    {
        public string Count { get; set; }

        public string Message { get; set; }

        public int Result { get; set; }

        public long Time { get; set; }
        public T Data { get; set; }
    }
}
