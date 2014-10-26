using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EasyGameServer.PacketHandlers
{
    class LoginHandler : PacketHandler
    {

        void PacketHandler.Run(int cridential, string payload)
        {
			LoginResult loginResult = JsonFx.Json.JsonReader.Deserialize<LoginResult>(payload);
			PlayerManager.GetInstance().m_MyPlayerID = loginResult.m_PlayerID;
			NetworkManager.GetInstance().LoginJobDone();

        }
    }
}
