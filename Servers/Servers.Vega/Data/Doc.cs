using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servers.Vega.Data
{
    public class Doc
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }

        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

    }
}
