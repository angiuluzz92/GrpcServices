using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TestGRpc.Services
{
    public class ActionService : Actioneer.ActioneerBase
    {
        public override Task<ActionReply> SayAction(ActionRequest request, ServerCallContext context)
        {
            ActionReply r = new ActionReply();
            r.Risultato = "OK SayAction";

            return Task.FromResult(r);

        }

        public override Task<FileExistsReply> IsFileExists(Empty e, ServerCallContext context) // IServerStreamWriter<DipendenteReply> s,
        {
            string curFile = @"c:\temp\test.txt";
            FileExistsReply res = new FileExistsReply();
            res.Risultato = File.Exists(curFile) == true ? "OK" : "KO";
            return Task.FromResult(res);

            //Console.WriteLine(File.Exists(curFile) ? "File exists." : "File does not exist.");
        }

        public override async Task AddEventFileExists(Empty e, IServerStreamWriter<FileExistsReply> s, ServerCallContext context)
        {
            string curFile = @"c:\temp\test.txt";
            FileExistsReply res = new FileExistsReply();
            while (File.Exists(curFile))
            {
                res.Risultato = "KO";
                s.WriteAsync(res);
                
            }
            res.Risultato = "OK";
            s.WriteAsync(res);
        }
    }
}