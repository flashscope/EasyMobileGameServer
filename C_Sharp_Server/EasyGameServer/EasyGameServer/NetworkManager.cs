using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasyGameServer
{

    class NetworkManager
    {

        private static Socket m_ListenSocket = null;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public bool Initialize()
        {

            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Defines.LISTEN_PORT);

            // Create a TCP/IP socket.
            m_ListenSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                m_ListenSocket.Bind(localEndPoint);
                m_ListenSocket.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            Console.WriteLine("SERVER ON");
            
            return true;
        }


        public void StartAccept()
        {
            while ( true )
            {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                Console.WriteLine("Waiting for a connection...");
                m_ListenSocket.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    m_ListenSocket);

                // Wait until a connection is made before continuing.
                allDone.WaitOne();
            }
            
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket socket = listener.EndAccept(ar);

            // Create the state object.
            ClientConnection client = new ClientConnection();
            client.workSocket = socket;

            socket.BeginReceive(client.buffer, 0, ClientConnection.BufferSize, 0,
                new AsyncCallback(client.ReadCallback), client);

            Console.WriteLine("ACCEPT DONE [{0}]", socket.Handle);
        }

    }
}
