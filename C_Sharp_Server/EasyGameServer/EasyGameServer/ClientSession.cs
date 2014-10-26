using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer
{
    public class ClientSession
    {
        public int m_PlayerID = -1;
        public float m_PosX = float.MinValue;
        public float m_PosY = float.MinValue;
        public float m_PosZ = float.MinValue;
        public float m_Angle = float.MinValue;
        public float m_Speed = float.MinValue;

        public int m_Credential = -1;
        private ClientConnection m_ClientConnection = null;
        public int m_LastHeartBeatedTick = Environment.TickCount;

        public ClientConnection GetConnection()
        {
            return m_ClientConnection;
        }

        public void SetConnection(ClientConnection connection)
        {
            m_ClientConnection = connection;
        }

        public void DisconnectConnection()
        {
            if( null != m_ClientConnection)
            {
                try
                {
                    if (null != m_ClientConnection.workSocket)
                    {
                        m_ClientConnection.workSocket.Disconnect(false);
                        m_ClientConnection.workSocket.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }


        public bool IsTimeOut()
        {
            if (Environment.TickCount - m_LastHeartBeatedTick > Defines.HEARTBEAT_TIMEOUT)
            {
                return true;
            }
            return false;
        }

        public void ResetTickTime()
        {
            m_LastHeartBeatedTick = Environment.TickCount;
        }

    }
}
