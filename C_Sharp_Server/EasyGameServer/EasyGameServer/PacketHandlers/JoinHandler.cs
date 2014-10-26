using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer.PacketHandlers
{
    class JoinHandler : PacketHandler
    {
        void PacketHandler.Run(int credential, string payload)
        {
            ClientSession clientSession = EasyGameServer.g_WorldManager.GetClient(credential);
            JoinRequest joinRequest = JsonFx.Json.JsonReader.Deserialize<JoinRequest>(payload);


            JoinResult joinResultPay = new JoinResult();
            joinResultPay.m_PlayerID = clientSession.m_PlayerID;
            joinResultPay.m_PosX = clientSession.m_PosX;
            joinResultPay.m_PosY = clientSession.m_PosY;
            joinResultPay.m_PosZ = clientSession.m_PosZ;
            joinResultPay.m_Angle = clientSession.m_Angle;
            joinResultPay.m_Speed = clientSession.m_Speed;

            string resultPayload = JsonFx.Json.JsonWriter.Serialize(joinResultPay);
            resultPayload = Utils.Base64Encoding(resultPayload);

            Packet joinResult = new Packet();
            joinResult.m_Type = (int)PacketTypes.PKT_SC_JOIN;
            joinResult.m_Payload = resultPayload;
            string resultPacket = JsonFx.Json.JsonWriter.Serialize(joinResult);

            
            EasyGameServer.g_WorldManager.SendBroadCast(resultPacket);
        }

    }

}
