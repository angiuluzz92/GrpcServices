using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAppTest.Pages
{
    public partial class Alarms
    {

        private List<TagMessage> alarms = new List<TagMessage>();

        protected override async Task OnInitializedAsync()
        {
            var client = new Tags.TagsClient(Channel);

            var cts = new CancellationTokenSource();
            using (var stream = client.GetTagsStream(new EnumerateRequest(), cancellationToken: cts.Token))
            {
                while (await stream.ResponseStream.MoveNext(cancellationToken: cts.Token))
                {
                    Console.WriteLine("Aggiunto: " + stream.ResponseStream.Current);
                    alarms.Add(stream?.ResponseStream?.Current);
                    StateHasChanged();
                }
            }
            //using var streamingCall = client.GetTagsStream(new EnumerateRequest());
            //try
            //{
            //    await foreach (var data in streamingCall.ResponseStream.ReadAllAsync(cts.Token))
            //    {
            //        //RaiseAlarm(data.Tag, data.Value);
            //        alarms = alarms.Add(data);
            //        StateHasChanged();
            //    }
            //}
            //catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            //{
            //    // do nothing atm
            //    var test = ex.Message;
            //}
            //catch (Exception ex)
            //{
            //    var pippo = ex.Message;
            //}
        }

    }
}
