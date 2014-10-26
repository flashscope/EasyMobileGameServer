using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer.PacketHandlers
{
    public class MoveHandler : PacketHandler
    {

        void PacketHandler.Run(int credential, string payload )
        {
            ClientSession clientSession = EasyGameServer.g_WorldManager.GetClient(credential);
            MoveRequest moveRequest = JsonFx.Json.JsonReader.Deserialize<MoveRequest>(payload);

            clientSession.m_PosX = moveRequest.m_PosX;
            clientSession.m_PosY = moveRequest.m_PosY;
            clientSession.m_PosZ = moveRequest.m_PosZ;
            clientSession.m_Angle = moveRequest.m_Angle;
            clientSession.m_Speed = moveRequest.m_Speed;


            MoveResult moveResultPay = new MoveResult();
            moveResultPay.m_PlayerID = clientSession.m_PlayerID;
            moveResultPay.m_PosX = clientSession.m_PosX;
            moveResultPay.m_PosY = clientSession.m_PosY;
            moveResultPay.m_PosZ = clientSession.m_PosZ;
            moveResultPay.m_Angle = clientSession.m_Angle;
            moveResultPay.m_Speed = clientSession.m_Speed;

            string resultPayload = JsonFx.Json.JsonWriter.Serialize(moveResultPay);
            resultPayload = Utils.Base64Encoding(resultPayload);

            Packet moveResult = new Packet();
            moveResult.m_Type = (int)PacketTypes.PKT_SC_MOVE;
            moveResult.m_Payload = resultPayload;
            string resultPacket = JsonFx.Json.JsonWriter.Serialize(moveResult);

            EasyGameServer.g_WorldManager.SendBroadCast(resultPacket);
        }
    }

    

}
