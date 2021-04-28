using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestGRpc.Services
{
    public class TagService : Tags.TagsBase
    {

        public async Task GetTagsStream(Empty e, IServerStreamWriter<TagMessage> s, ServerCallContext context)
        {
            for (int i = 0; i < 6; i++)
            {
                TagMessage tag = new TagMessage();
                tag.Description = "Prova tag " + i;
                tag.Id = "15";
                tag.Name = "TAG-" + i;
                tag.Value = "Valore test";
                await Task.Delay(3000);
                await s.WriteAsync(tag);
            }
        }

    }
}
