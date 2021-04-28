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
        public override async Task<AlarmList> GetAlarmList(AlarmListRequest e,  ServerCallContext context)
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
            }

            AlarmList al = new AlarmList();
            al.Alarms.Add(alarms);
            return al;

        }

        public override async Task GetAlarmStream(Empty e, IServerStreamWriter<AlarmProto> s, ServerCallContext context)
        {           
            for (int i = 0; i < 6; i++)
            {
                AlarmProto alarm = new AlarmProto();
                alarm.Ack = false;
                alarm.AckDate = new Timestamp();
                alarm.AckUser = "ADM";
                alarm.Active = true;
                alarm.Count = 49;
                alarm.Description = "Centrale / Area 01 / Status";
                alarm.EndDate = new Timestamp();
                alarm.Flag = "";
                alarm.Gravity = 50;
                alarm.GroupDescription = "CIABI_ELMO";
                alarm.GroupId = 2;
                alarm.Id = 16;
                alarm.Info = "";
                alarm.PreAck = false;
                alarm.PreAckDate = new Timestamp();
                alarm.PreAckUser = "";
                alarm.ReportId = 1;
                alarm.StartDate = new Timestamp();
                alarm.Type = 1;
                alarm.VariableId = 65;
                alarm.VariableStatusDescription = "Disinserimento " + i;
                await Task.Delay(3000);
                await s.WriteAsync(alarm);
            }
        }
    }
}
