using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer.PacketHandlers
{
    class HeartBeatHandler : PacketHandler
    {
        void PacketHandler.Run(int credential, string payload)
        {
            HeartBeatResult heartBeatResultPay = new HeartBeatResult();
            string resultPayload = JsonFx.Json.JsonWriter.Serialize(heartBeatResultPay);
            resultPayload = Utils.Base64Encoding(resultPayload);

            Packet heartBeatResult = new Packet();
            heartBeatResult.m_Type = (int)PacketTypes.PKT_SC_PONG;
            heartBeatResult.m_Payload = resultPayload;
            heartBeatResult.m_Credential = credential;

            string resultPacket = JsonFx.Json.JsonWriter.Serialize(heartBeatResult);

            EasyGameServer.g_WorldManager.SendMessage(credential, resultPacket);
        }
    }
}
