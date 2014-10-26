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
    public class ClientConnection
    {
        // no variable here
        // Client  socket.
        public Socket workSocket = null;


        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        private StringBuilder sb = new StringBuilder();


        private int m_ErrorCount = 0;
        private PacketHandler[] m_PacketHandlerList = new PacketHandler[(int)PacketTypes.PKT_MAX];

        public ClientConnection()
        {
            RegistHandler();
        }

        private void RegistHandler()
        {
            // will make dll dynamic load
            try
            {
                {
                    Type type = Type.GetType("EasyGameServer.PacketHandlers.LoginHandler", true);
                    PacketHandler handler = (PacketHandler)Activator.CreateInstance(type);
                    m_PacketHandlerList[(int)PacketTypes.PKT_CS_LOGIN] = handler;
                }
                {
                    Type type = Type.GetType("EasyGameServer.PacketHandlers.JoinHandler", true);
                    PacketHandler handler = (PacketHandler)Activator.CreateInstance(type);
                    m_PacketHandlerList[(int)PacketTypes.PKT_CS_JOIN] = handler;
                }
                {
                    Type type = Type.GetType("EasyGameServer.PacketHandlers.MoveHandler", true);
                    PacketHandler handler = (PacketHandler)Activator.CreateInstance(type);
                    m_PacketHandlerList[(int)PacketTypes.PKT_CS_MOVE] = handler;
                }
                {
                    Type type = Type.GetType("EasyGameServer.PacketHandlers.HeartBeatHandler", true);
                    PacketHandler handler = (PacketHandler)Activator.CreateInstance(type);
                    m_PacketHandlerList[(int)PacketTypes.PKT_CS_PING] = handler;
                }
                {
                    Type type = Type.GetType("EasyGameServer.PacketHandlers.SyncHandler", true);
                    PacketHandler handler = (PacketHandler)Activator.CreateInstance(type);
                    m_PacketHandlerList[(int)PacketTypes.PKT_CS_SYNC] = handler;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                m_ErrorCount = 0;

                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                ClientConnection client = (ClientConnection)ar.AsyncState;
                Socket socket = client.workSocket;

                // Read data from the client socket. 
                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {



                    // 여길 어떻게 처리해야할까!!! 서큘라!!
                    // There  might be more data, so store the data received so far.
                    client.sb.Append(Encoding.ASCII.GetString(
                        client.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = client.sb.ToString();

                    int eofPos = content.IndexOf("{EOF}");
                    if (eofPos > -1)
                    {
                        content = content.Remove(eofPos);
                        sb.Remove(0, eofPos+5);
                        //sb.Clear();
                        Console.WriteLine("["+sb.ToString()+"]");
                        // All the data has been read from the 
                        // client. Display it on the console.
                        Console.WriteLine("Read {0} bytes from socket[{1}] Data : {2}",
                            content.Length, socket.Handle, content);
                        // Echo the data back to the client.
                        //Send(client, content);

                        PacketParser(content);
                    }
                    else
                    {
                        // Not all data received. Get more.
                        socket.BeginReceive(client.buffer, 0, ClientConnection.BufferSize, 0,
                        new AsyncCallback(ReadCallback), client);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(String data)
        {
            try
            {
                Socket socket = workSocket;

                data += "{EOF}";

                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                socket.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), this);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ++m_ErrorCount;

                try
                {
                    if (m_ErrorCount > Defines.CONNECTION_ERROR_MAX)
                    {
                        workSocket.Disconnect(false);
                        workSocket.Close();
                        workSocket = null;
                    }
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.ToString());
                }
            }


            
        }

        private void SendCallback(IAsyncResult ar)
        {
            ClientConnection client = (ClientConnection)ar.AsyncState;
            Socket socket = client.workSocket;
            try
            {
                m_ErrorCount = 0;

                // Complete sending the data to the remote device.
                int bytesSent = socket.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client[{1}]", bytesSent, socket.Handle);


                socket.BeginReceive(client.buffer, 0, ClientConnection.BufferSize, 0,
                    new AsyncCallback(ReadCallback), client);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private void PacketParser(string packetData)
        {
            try
            {
                Packet packet = JsonFx.Json.JsonReader.Deserialize<Packet>(packetData);

                int credential = packet.m_Credential;
                PacketTypes packetType = (PacketTypes)packet.m_Type;

                string payload = Utils.Base64Decoding(packet.m_Payload);

                if ( packetType == PacketTypes.PKT_CS_LOGIN )
                {
                    credential = EasyGameServer.g_WorldManager.RegistClientSession(credential, this);
                }

                Console.WriteLine("packetType:" + packetType);
                EasyGameServer.g_WorldManager.ResetTickTime(credential);
                m_PacketHandlerList[(int)packetType].Run(credential, payload);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            

        }


    }
}
