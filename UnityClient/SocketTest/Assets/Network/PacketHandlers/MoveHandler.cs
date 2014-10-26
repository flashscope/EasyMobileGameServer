using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyGameServer.PacketHandlers
{
    public class MoveHandler : PacketHandler
    {

        void PacketHandler.Run(int cridential, string payload )
        {
			MoveResult moveResult = JsonFx.Json.JsonReader.Deserialize<MoveResult>(payload);
			PlayerManager.GetInstance().AddMoveResult(moveResult);
        }
    }

    

}
