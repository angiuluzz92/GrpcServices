using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestGRpc.Services
{
    public class AlarmService : Alarmer.AlarmerBase
    {
        public override async Task GetStreamAlarmList(AlarmListRequest e, IServerStreamWriter<AlarmProto> s, ServerCallContext context)
        {
            for (int i = 0; i < 6; i++)
            {
                AlarmProto a = new AlarmProto();

                a.Ack = false;
                a.AckDate = new Timestamp();
                a.AckUser = "ADM";
                a.Active = true;
                a.Count = 49;
                a.Description = "Centrale / Area 01 / Status";
                a.EndDate = new Timestamp();
                a.Flag = "";
                a.Gravity = 50;
                a.GroupDescription = "CIABI_ELMO";
                a.GroupId = 2;
                a.Id = 16;
                a.Info = "";
                a.PreAck = false;
                a.PreAckDate = new Timestamp();
                a.PreAckUser = "";
                a.ReportId = 1;
                a.StartDate = new Timestamp();
                a.Type = 1;
                a.VariableId = 65;
                a.VariableStatusDescription = "Disinserimento " + i;

                await Task.Delay(15000);
                await s.WriteAsync(a);
            }
        }

        public override async Task GetAlarmList(AlarmListRequest e, IServerStreamWriter<AlarmList> s, ServerCallContext context)
        {
            AlarmList alarmListR = new AlarmList();
            List<AlarmProto> alarms = new List<AlarmProto>();
            for (int i = 0; i < 6; i++)
            {
                AlarmProto a = new AlarmProto();
                a.Ack = false;
                a.AckDate = new Timestamp();
                a.AckUser = "ADM";
                a.Active = true;
                a.Count = 49;
                a.Description = "Centrale / Area 01 / Status";
                a.EndDate = new Timestamp();
                a.Flag = "";
                a.Gravity = 50;
                a.GroupDescription = "CIABI_ELMO";
                a.GroupId = 2;
                a.Id = 16;
                a.Info = "";
                a.PreAck = false;
                a.PreAckDate = new Timestamp();
                a.PreAckUser = "";
                a.ReportId = 1;
                a.StartDate = new Timestamp();
                a.Type = 1;
                a.VariableId = 65;
                a.VariableStatusDescription = "Disinserimento " + i;

                alarms.Add(a);
                alarmListR.Alarms.Add(alarms);
                await Task.Delay(3000);
                await s.WriteAsync(alarmListR);

                //AlarmList alarmListR = new AlarmList();
                //alarmListR.Alarms.Add(alarms);
                //return alarmListR;
            }

        }
    }
}
