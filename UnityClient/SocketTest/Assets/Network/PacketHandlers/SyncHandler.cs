using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyGameServer.PacketHandlers
{
	class SyncHandler : PacketHandler
	{
		void PacketHandler.Run(int cridential, string payload)
		{
			SyncResult syncResult = JsonFx.Json.JsonReader.Deserialize<SyncResult>(payload);
			List<JoinResult> joinList = syncResult.m_JoinList;

			Logger.GetInstance().Log("Sync! count:"+joinList.Count);

			PlayerManager.GetInstance().ManagerClear();

			foreach (JoinResult joinResult in joinList)
			{
				PlayerManager.GetInstance().AddNewPlayer(joinResult);
			}

		}
		
	}
	
}
