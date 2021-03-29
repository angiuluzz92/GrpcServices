
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Windows;
using TestGRpc;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        public static Greeter.GreeterClient service = new Greeter.GreeterClient(channel);
        public static Actioneer.ActioneerClient serviceAction = new Actioneer.ActioneerClient(channel);
        public static Alarmer.AlarmerClient serviceAl = new Alarmer.AlarmerClient(channel);

        public MainWindow()
        {
            InitializeComponent();

            Empty e = new Empty();
            var response = service.SayHelloAsync(new HelloRequest() { Name = "Giovanni" }).ResponseAsync.Result;
            //listaDipendenti = service.GetAllDipendenti(e);
            //var tes = "";
        }

        private static async void getEventStatus()
        {
            Empty en = new Empty();
            using var streamF = serviceAction.AddEventFileExists(en);
            var lstResponse = streamF.ResponseStream.ReadAllAsync();
            await foreach (var r in lstResponse)
            {
                eventStatus = r.Risultato + " - orario: " + DateTime.Now;
            }
        }

        private static async void getListaDip()
        {
            Empty en = new Empty(); 
            using var streamF = service.GetAllDipendenti(en);
            var lstResponse = streamF.ResponseStream.ReadAllAsync();
            await foreach (var r in lstResponse)

            {
                listaDipendenti.Add(r);
            }
        }

        private void getEvent_Click(object sender, RoutedEventArgs e)
        {
            getEventStatus();
        }

        private void getLista_Click(object sender, RoutedEventArgs e)
        {
            getListaDip();
        }

        private static  List<DipendenteReply> listaDipendenti = new List<DipendenteReply>();
        private static string eventStatus = "";
    }
}
