using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestGRpc
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override async Task Calculate(IAsyncStreamReader<BidirectionalCalculatorRequest> requestStream, IServerStreamWriter<BidirectionalCalculatorReply> responseStream, ServerCallContext context)
        {
            double result = 0.0D;
            string equation = result.ToString();

            await foreach (var message in requestStream.ReadAllAsync())
            {
                var request = requestStream.Current;
                double n = request.N;
                switch (request.Operation)
                {
                    case Operation.Add:
                        result += n;
                        equation += " + " + n.ToString();
                        break;
                    case Operation.Subtract:
                        result -= n;
                        equation += " - " + n.ToString();
                        break;
                    case Operation.Multiply:
                        result *= n;
                        equation += " * " + n.ToString();
                        break;
                    case Operation.Divide:
                        if (n == 0)
                            throw new RpcException(new Status(StatusCode.InvalidArgument, "Divide by zero attempt."));
                        result /= n;
                        equation += " / " + n.ToString();
                        break;
                    case Operation.Clear:
                        equation += " = " + result.ToString();
                        break;
                    default:
                        continue;
                }
                await responseStream.WriteAsync(new BidirectionalCalculatorReply()
                {
                    Result = result,
                    Eqn = equation
                }).ConfigureAwait(false);

                //reset state
                if (request.Operation == Operation.Clear)
                {
                    result = 0.0D;
                    equation = result.ToString();
                }
            }
        }

        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello Ciao" + request.Name
            });
        }

        public override Task<SurnameReply> SaySurname(SurnameRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SurnameReply
            {
                MessageSurname = "Cognome: " + request.Surname
            });
        }

        public override Task<PersonaReply> SavePersona(PersonaRequest request, ServerCallContext context)
        {
            if (request.Uomo)
            {
                return Task.FromResult(new PersonaReply
                {
                    //TODO Salva persona su DB
                    Result = $"{request.Name} {request.Surname} è stato salvato"
                });
            }
            else
            {
                return Task.FromResult(new PersonaReply
                {
                    //TODO Salva persona su DB
                    Result = $"Errore, {request.Name} {request.Surname} NON è stato salvato"
                });
            }
        }

        public override Task<DipendenteReply> GetDipendente(DipendenteRequest request, ServerCallContext context)
        {
            //return Task.FromResult(d);
            return Task.FromResult(new DipendenteReply
            {
                Comune = "Roma",
                Nominativo = "Giovanni Appugliese"

            }); ;

        }

        public override async Task GetAllDipendenti(Empty e, IServerStreamWriter<DipendenteReply> s, ServerCallContext context)
        {
            List<DipendenteReply> dip = new List<DipendenteReply>();
            for(int i=0; i<6;i++)
            {
                DipendenteReply d = new DipendenteReply();
                d.Comune = "Roma " + i.ToString();
                d.Nominativo = "Giovanni " + i.ToString();
                await Task.Delay(3000);
                await s.WriteAsync(d);
            }
        }

        public override async Task<DipendentiReply> GetAllDipendentiList(DipendenteRequest request, ServerCallContext context)
        {
            List<DipendenteReply> dips = new List<DipendenteReply>();
            
            for (int i = 0; i < 5; i++)
            {
                DipendenteReply d = new DipendenteReply();
                d.Comune = "Roma " + i.ToString();
                d.Nominativo = "GioAppu" + i.ToString();
                dips.Add(d);
            }
            
            DipendentiReply dipend = new DipendentiReply();            
            dipend.ListaDipendenti.Add(dips);
            return dipend;                         
        }
    }
}
