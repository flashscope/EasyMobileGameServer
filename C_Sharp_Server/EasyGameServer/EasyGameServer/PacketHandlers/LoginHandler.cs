using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer.PacketHandlers
{
    class LoginHandler : PacketHandler
    {
        private static int testPlayerID = 1; // test variable

        void PacketHandler.Run(int credential, string payload)
        {

            ClientSession clientSession = EasyGameServer.g_WorldManager.GetClient(credential);

            LoginRequest loginRequest = JsonFx.Json.JsonReader.Deserialize<LoginRequest>(payload);

            LoginResult loginResultPay = new LoginResult();

            string userKey = loginRequest.m_UserKey;
            
            loginResultPay.m_PlayerID = credential; // actually from DB

            ++testPlayerID; // test...
            int value = testPlayerID % 4;
            clientSession.m_PlayerID = credential;
            clientSession.m_PosX = value / 10.0f;// from DB
            clientSession.m_PosY = value / 10.0f;// from DB
            clientSession.m_PosZ = value / 10.0f;// from DB
            clientSession.m_Angle = testPlayerID;// from DB
            clientSession.m_Speed = 1;// from DB


            string resultPayload = JsonFx.Json.JsonWriter.Serialize(loginResultPay);
            resultPayload = Utils.Base64Encoding(resultPayload);

            Packet loginResult = new Packet();
            loginResult.m_Type = (int)PacketTypes.PKT_SC_LOGIN;
            loginResult.m_Payload = resultPayload;
            loginResult.m_Credential = credential;

            string resultPacket = JsonFx.Json.JsonWriter.Serialize(loginResult);

            EasyGameServer.g_WorldManager.SendMessage(credential, resultPacket);
        }
    }
}
