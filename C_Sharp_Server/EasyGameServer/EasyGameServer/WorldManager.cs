using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;

namespace EasyGameServer
{
    class WorldManager
    {
        private int m_WorldTick = Environment.TickCount;
        private int m_GCTick = Environment.TickCount;

        private Dictionary<int, ClientSession> m_ClientList = new Dictionary<int, ClientSession>();
        private List<ClientSession> m_GuillotineList = new List<ClientSession>();

        private System.Object lockThis = new System.Object();


        public WorldManager()
        {
            Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(OnPeriodWork);
            myTimer.Interval = 10;
            myTimer.Start();
        }




        public int RegistClientSession(int credential, ClientConnection connection)
        {
            lock (lockThis)
            {
                if (!m_ClientList.ContainsKey(credential))
                {
                    if (credential == -1)
                    {
                        credential = EasyGameServer.g_credentialManager.GetCredential();
                        ClientSession client = new ClientSession();
                        client.m_Credential = credential;
                        m_ClientList.Add(credential, client);
                    }
                    else
                    {
                        // 정리되어버림
                        // 그냥 세션만들어 주던지 에러 페킷을 주던지...
                    }
                    
                }

                m_ClientList[credential].SetConnection(connection);
            }

            return credential;
        }


        public void DeleteClient(int credential)
        {
            lock (lockThis)
            {
                if (m_ClientList.ContainsKey(credential))
                {
                    m_ClientList[credential].DisconnectConnection();

                    m_ClientList.Remove(credential);

                    EasyGameServer.g_credentialManager.ReturnCredential(credential);
                }
            }
        }

        public ClientSession GetClient(int credential)
        {
            return m_ClientList[credential];
        }

        // need now?
        public Dictionary<int, ClientSession> GetClientList()
        {
            return m_ClientList;
        }

        public bool IsSessionContain(int credential)
        {
            if (m_ClientList.ContainsKey(credential))
            {
                return true;
            }
            return false;
        }

        public void ResetTickTime(int credential)
        {
            try
            {
                m_ClientList[credential].ResetTickTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }


        public void SendMessage(int credential, string message)
        {
            try
            {
                if (null != m_ClientList[credential].GetConnection())
                {
                    m_ClientList[credential].GetConnection().Send(message);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        public void SendBroadCast(string message)
        {
            foreach (ClientSession client in m_ClientList.Values)
            {
                if ( null != client.GetConnection() )
                {
                    if (null != client.GetConnection().workSocket)
                    {
                        client.GetConnection().Send(message);
                    }
                }
            }
        }


        public string WrapPacket(PacketTypes packetType, string payload)
        {
            payload = Utils.Base64Encoding(payload);

            Packet packet = new Packet();
            packet.m_Type = (int)packetType;
            packet.m_Payload = payload;
            string resultPacket = JsonFx.Json.JsonWriter.Serialize(packet);

            return resultPacket;
        }



        public void OnPeriodWork(Object myObject, EventArgs myEventArgs)
        {
            m_WorldTick = Environment.TickCount;

            if (m_WorldTick - m_GCTick > Defines.GC_INTERVAL)
            {
                CollectGarbageSessions();
                m_GCTick = m_WorldTick;
            }
            
        }

        public void CollectGarbageSessions()
        {

            Console.WriteLine("CollectGarbageSessions");

            m_GuillotineList.Clear();

            foreach (ClientSession client in m_ClientList.Values)
            {
                if ( client.IsTimeOut() )
                {
                    m_GuillotineList.Add(client);
                }
            }

            for(int i=0; i<m_GuillotineList.Count; ++i)
            {
                try
                {
                    ClientSession client = m_GuillotineList[i];
                    Console.WriteLine("Delete!!");
                    DeleteClient(client.m_Credential);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

        }

    }
}
