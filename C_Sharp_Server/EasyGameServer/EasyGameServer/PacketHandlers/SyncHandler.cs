using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer.PacketHandlers
{
    class SyncHandler : PacketHandler
    {
        void PacketHandler.Run(int credential, string payload)
        {

            Dictionary<int, ClientSession> clientList = EasyGameServer.g_WorldManager.GetClientList();
            List<JoinResult> joinList = new List<JoinResult>();

            foreach (ClientSession client in clientList.Values)
            {
                JoinResult joinResultPay = new JoinResult();
                joinResultPay.m_PlayerID = client.m_PlayerID;
                joinResultPay.m_PosX = client.m_PosX;
                joinResultPay.m_PosY = client.m_PosY;
                joinResultPay.m_PosZ = client.m_PosZ;
                joinResultPay.m_Angle = client.m_Angle;
                joinResultPay.m_Speed = client.m_Speed;


                joinList.Add(joinResultPay);
            }

            SyncResult syncResultPay = new SyncResult();
            syncResultPay.m_JoinList = joinList;


            string resultPayload = JsonFx.Json.JsonWriter.Serialize(syncResultPay);

            Console.WriteLine("from:" + credential +"!" + resultPayload);

            string resultPacket = EasyGameServer.g_WorldManager.WrapPacket(PacketTypes.PKT_SC_SYNC, resultPayload);
            Console.WriteLine(":"+resultPacket);
            EasyGameServer.g_WorldManager.SendMessage(credential, resultPacket);
        }
    }
}
