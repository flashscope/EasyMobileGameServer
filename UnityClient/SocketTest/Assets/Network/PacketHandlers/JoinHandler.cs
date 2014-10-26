using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyGameServer.PacketHandlers
{
    class JoinHandler : PacketHandler
    {
        void PacketHandler.Run(int cridential, string payload)
        {
			JoinResult joinResult = JsonFx.Json.JsonReader.Deserialize<JoinResult>(payload);
			PlayerManager.GetInstance().AddNewPlayer(joinResult);
        }

    }

}
