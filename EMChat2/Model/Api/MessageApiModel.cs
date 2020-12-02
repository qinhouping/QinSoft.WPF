using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    public class MessageContentApiModel
    {
        public string Type { get; set; }

        public string Content { get; set; }
    }

    public class MixedMessageContentApiModel
    {
        public MessageContentApiModel[] Items { get; set; }
    }

    public class MessageApiModel : MessageContentApiModel
    {
        public string Id { get; set; }

        public DateTime Time { get; set; }

        public string ChatId { get; set; }

        public string FromUser { get; set; }

        public string[] ToUsers { get; set; }

        public int State { get; set; }
    }
}
