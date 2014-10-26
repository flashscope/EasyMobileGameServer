using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EasyGameServer
{
    class EasyGameServer
    {
        public static credentialManager g_credentialManager = null;
        public static WorldManager g_WorldManager = null;

        private static NetworkManager m_NetworkManager = null;

        private static Monitor m_Monitor = null;
        private static Thread m_MonitorThread = null;

        public void Run()
        {
            if (g_credentialManager == null)
            {
                g_credentialManager = new credentialManager();
            }

            if ( g_WorldManager == null )
            {
                g_WorldManager = new WorldManager();
            }

            if (m_Monitor == null)
            {
                m_Monitor = new Monitor(15000);
                m_MonitorThread = new Thread(m_Monitor.Run);
                m_MonitorThread.Start();
            }
            

            if (m_NetworkManager == null)
            {
                m_NetworkManager = new NetworkManager();
                if (!m_NetworkManager.Initialize())
                {
                    Console.WriteLine("[ERROR]networkManager.Initialize");
                }

                // block here
                m_NetworkManager.StartAccept();
            }


        }
    }
}
