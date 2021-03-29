using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestGRpc
{
    public class GreeterService : Greeter.GreeterBase
    {
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
