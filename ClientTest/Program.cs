using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestGRpc;

namespace ClientTest
{
    class Program
    {
        public static Empty e = new Empty();
        public static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        public static Greeter.GreeterClient service = new Greeter.GreeterClient(channel);
        public static Actioneer.ActioneerClient serviceAction = new Actioneer.ActioneerClient(channel);

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            bool keepLooping = true;



            while (keepLooping)
            {
                string command = Console.ReadLine();
                switch (command)
                {
                    case "hello":
                        SayHelloAsync();
                        Console.WriteLine("SayHelloAsync() end");
                        break;
                    case "surname":
                        SaySurnameAsync();
                        Console.WriteLine("SaySurnameAsync() end");
                        break;
                    case "action":
                        SayActionAsync();
                        Console.WriteLine("SayActionAsync() end");
                        break;
                    case "getalldipendenti":
                        GelAllDipendentiAsync();
                        Console.WriteLine("GelAllDipendentiAsync() end");
                        break;
                    case "dipendentilist":
                        GelAllDipendentiListAsync();
                        Console.WriteLine("GelAllDipendentiListAsync() end");
                        break;
                    case "fileexists":
                        IsFileExistsAsync();
                        Console.WriteLine("IsFileExistsAsync() end");
                        break;
                    case "eventfile":
                        EventFileExistsAsync();
                        break;
                    case "exit":
                        keepLooping = false;
                        break;
                    case "calcola":
                        CalculateTest();
                        break;
                    default:
                        break;
                }
            }



            Console.ReadKey();
        }

        public static async void CalculateTest()
        {
            //BidirectionalCalculatorServiceClient bidirectionalClient = new BidirectionalCalculatorServiceClient(channel);
            using (var duplexStream = service.Calculate())
            {
                var callbackHandler = new CallbackHandler(duplexStream.ResponseStream);

                await Task.Delay(3000);
                await duplexStream.RequestStream.WriteAsync(new BidirectionalCalculatorRequest()
                {
                    Operation = Operation.Add,
                    N = 2
                });

                await Task.Delay(3000);
                await duplexStream.RequestStream.WriteAsync(new BidirectionalCalculatorRequest()
                {
                    Operation = Operation.Multiply,
                    N = 2
                });

                await Task.Delay(3000);
                await duplexStream.RequestStream.WriteAsync(new BidirectionalCalculatorRequest()
                {
                    Operation = Operation.Subtract,
                    N = 2
                });

                await Task.Delay(3000);
                await duplexStream.RequestStream.WriteAsync(new BidirectionalCalculatorRequest()
                {
                    Operation = Operation.Clear
                });

                await Task.Delay(3000);
                await duplexStream.RequestStream.WriteAsync(new BidirectionalCalculatorRequest()
                {
                    Operation = Operation.Add,
                    N = 10
                });

                await Task.Delay(3000);
                await duplexStream.RequestStream.CompleteAsync();
                await callbackHandler.Task;
            }
        }


        public static async void EventFileExistsAsync()
        {
            using var streamF = serviceAction.AddEventFileExists(e);  
            var lstResponse = streamF.ResponseStream.ReadAllAsync();
            await foreach (var r in lstResponse)
            {
                Console.WriteLine("File non esiste, res " + r.Risultato);
            }
        }

        public static async void IsFileExistsAsync()
        {
            FileExistsReply files = new FileExistsReply();
            FileExistsReply resFile = await serviceAction.IsFileExistsAsync(e);
            Console.WriteLine(resFile.Risultato);
        }

        public static async void SayHelloAsync()
        {
            HelloReply response = await service.SayHelloAsync(new HelloRequest() { Name = "Giovanni" });
            Console.WriteLine(response.Message);
        }

        public static async void SaySurnameAsync()
        {
            var responseSurname = await service.SaySurnameAsync(new SurnameRequest() { Surname = "Appugliese" });
            Console.WriteLine(responseSurname.MessageSurname);
        }

        public static async void SavePersonaAsync()
        {
            PersonaRequest personaRequest = new PersonaRequest();
            personaRequest.Surname = "Appugliese";
            personaRequest.Name = "Giovanni";
            personaRequest.Age = 34;
            personaRequest.Uomo = true;
            var responsePersona = await service.SavePersonaAsync(personaRequest);
            Console.WriteLine(responsePersona.Result);
        }

        public static async void SayActionAsync()
        {
            ActionReply ar = serviceAction.SayAction(new ActionRequest() { Azione = 3 });
            Console.WriteLine(ar.Risultato);
        }

        public static async void GelAllDipendentiAsync()
        {
            using var stream = service.GetAllDipendenti(e);
            List<DipendenteReply> listaDip = new List<DipendenteReply>();
            await foreach (var r in stream.ResponseStream.ReadAllAsync())
            {
                //listaDip.Add(r);
                Console.WriteLine(r.Comune + " " + r.Nominativo);
            }
        }

        public static async void GelAllDipendentiListAsync()
        {
            DipendenteRequest dr = new DipendenteRequest() { Ok = 1 };
            DipendentiReply drList = await service.GetAllDipendentiListAsync(dr);
            foreach (var dip in drList.ListaDipendenti)
            {
                Console.WriteLine(dip.Comune + " " + dip.Nominativo);
            }
        }

        internal class CallbackHandler
        {
            readonly IAsyncStreamReader<BidirectionalCalculatorReply> _responseStream;
            readonly CancellationToken _cancellationToken;

            public CallbackHandler(IAsyncStreamReader<BidirectionalCalculatorReply> responseStream) : this(responseStream, CancellationToken.None) { }

            public CallbackHandler(IAsyncStreamReader<BidirectionalCalculatorReply> responseStream, CancellationToken cancellationToken)
            {
                _responseStream = responseStream ?? throw new ArgumentNullException(nameof(responseStream));
                _cancellationToken = cancellationToken;
                Task = Task.Run(Consume, _cancellationToken);
            }

            public Task Task { get; }

            async Task Consume()
            {
                while (await _responseStream.MoveNext(_cancellationToken).ConfigureAwait(false))
                    Console.WriteLine("Equals({0}), Equation({1})", _responseStream.Current.Result, _responseStream.Current.Eqn);
            }
        }

    }
}
