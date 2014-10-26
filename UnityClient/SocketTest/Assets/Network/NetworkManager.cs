using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class StateObject {
	// Client socket.
	public Socket workSocket = null;
	// Size of receive buffer.
	public const int BufferSize = 1024;
	// Receive buffer.
	public byte[] buffer = new byte[BufferSize];
	// Received data string.
	public StringBuilder sb = new StringBuilder();
}



public class NetworkManager : MonoBehaviour {
	
	private static NetworkManager _instance = null;
	public static NetworkManager GetInstance()
	{
		return _instance;
	}
	
	void Start ()
	{
		if (_instance == null)
		{
			_instance = this;
			RegistHandler();
		}
        else
        {
            Destroy(gameObject);
        }
    }


	private DateTime m_LastHeartTime = DateTime.Now;
	private int m_SendedPingCount = 0;

	//private int m_JobCount = 0;
	public string m_MyUserKey = "NONE";
	public int m_MyCredential = -1;
	private bool m_IsJoined = false;
	private PacketHandler[] m_PacketHandlerList = new PacketHandler[(int)PacketTypes.PKT_MAX];
	private void RegistHandler()
	{
		m_PacketHandlerList[(int)PacketTypes.PKT_SC_LOGIN] = new EasyGameServer.PacketHandlers.LoginHandler();
		m_PacketHandlerList[(int)PacketTypes.PKT_SC_JOIN] = new EasyGameServer.PacketHandlers.JoinHandler();
		m_PacketHandlerList[(int)PacketTypes.PKT_SC_MOVE] = new EasyGameServer.PacketHandlers.MoveHandler();
		m_PacketHandlerList[(int)PacketTypes.PKT_SC_PONG] = new EasyGameServer.PacketHandlers.HeartBeatHandler();
		m_PacketHandlerList[(int)PacketTypes.PKT_SC_SYNC] = new EasyGameServer.PacketHandlers.SyncHandler();
        m_IsNetThreadRun = true;
        StartCoroutine( StartNetworkThread() );
    }


    void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 200, 100), "Conn"))
		{
			//StartLoginJob();
			StartCoroutine( StartLoginJob() );
		}
		/*
		if (GUI.Button (new Rect (0, 100, 200, 100), "Join"))
		{
			StartJoinJob();
        }
*/

	}


	private const int port = 9001;


	// The response from the remote device.
	private static String response = String.Empty;


	private static Socket m_Client = null;

	public static Action NetworkJob<T>(Action<T> action, T parameter)
	{
		return () => action(parameter);
	}

    //delegate void NetWorkJob(string str);
	private static List<Action> networkJobList = new List<Action>();


	private static bool m_IsNetThreadRun = false;
	public IEnumerator StartNetworkThread()
	{

		yield return null;

		while( m_IsNetThreadRun )
		{


			if(m_SendedPingCount == 0)
			{
				TimeSpan ts = DateTime.Now - m_LastHeartTime;
				
				if( ts.TotalSeconds > Defines.PING_CYCLE )
				{

					Debug.Log("StartPingJob");
					++m_SendedPingCount;
					//StartPingJob();
					StartCoroutine( StartLoginJob() );
                }
            }
			else if( m_SendedPingCount == 1 )
			{
				TimeSpan ts = DateTime.Now - m_LastHeartTime;
				if( ts.TotalSeconds > Defines.NETWORK_TIMEOUT )
				{
					Debug.LogError("NETWORK_TIMEOUT");
					Application.LoadLevel(Application.loadedLevel);
				}
            }
            
            
            
            
            try{
                if(networkJobList.Count > 0)
				{
					//if(m_JobCount == 0)
					{
						networkJobList[0]();
						networkJobList.RemoveAt(0);
					}

				}
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
        	}


        	
			//Debug.Log("job:"+networkJobList.Count+" ping:"+m_SendedPingCount+" JobCount:"+m_JobCount);

			yield return null;
		}
	}

	public void StartConnect(String data) // dummy parameter
	{
		Debug.Log(data);
		IPAddress ipAddress = IPAddress.Parse(Defines.SERVER_PATH);
		IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);



		// Create a TCP/IP socket.
		m_Client = new Socket(AddressFamily.InterNetwork,
		                           SocketType.Stream, ProtocolType.Tcp);

		//++m_JobCount;
		// Connect to the remote endpoint.
		m_Client.BeginConnect( remoteEP, 
		                    new AsyncCallback(ConnectCallback), m_Client);
		//connectDone.WaitOne();
	}

	private void ConnectCallback(IAsyncResult ar) {
		try {
			// Retrieve the socket from the state object.
			Socket client = (Socket) ar.AsyncState;
			
			// Complete the connection.
			client.EndConnect(ar);
			
			
			Debug.Log("Socket connected to "+client.RemoteEndPoint.ToString());
			//--m_JobCount;
			// Signal that the connection has been made.
			//connectDone.Set();
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}

	// not base64 payload
	public void SendPacket(PacketTypes packetType , string payload)
	{
		//Debug.Log("SendPacket:"+packetType+" listCount:"+networkJobList.Count);
		if(m_MyCredential == -1)
		{
			if(packetType != PacketTypes.PKT_CS_LOGIN)
			{
				return;
			}
		}
		
		payload = Utils.Base64Encoding(payload);
		
		Packet packet = new Packet();
		packet.m_Type = (int)packetType;
		packet.m_Payload = payload;
		packet.m_Credential = m_MyCredential;
		
		string packetData = JsonFx.Json.JsonWriter.Serialize(packet);


		networkJobList.Add( NetworkJob(Send, packetData) );
	}

	

	

	private void Send(String data)
	{
		data += "{EOF}";

		//++m_JobCount;
		//Debug.LogWarning(data);

		// Convert the string data to byte data using ASCII encoding.
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		
		// Begin sending the data to the remote device.
		m_Client.BeginSend(byteData, 0, byteData.Length, 0,
		                 new AsyncCallback(SendCallback), m_Client);

	}
	
	private void SendCallback(IAsyncResult ar) {
		try {
			// Retrieve the socket from the state object.
			Socket client = (Socket) ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = client.EndSend(ar);

			//Debug.Log("Sent " + bytesSent + " bytes to server.");

			// Receive the response from the remote device.
			Receive(client);

		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}


	private void Receive(Socket client) {
		try {
			// Create the state object.
			StateObject state = new StateObject();
			state.workSocket = client;
			
			// Begin receiving the data from the remote device.
			client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
			                    new AsyncCallback(ReceiveCallback), state);
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}
	
	private void ReceiveCallback( IAsyncResult ar ) {
		try {
			// Retrieve the state object and the client socket 
			// from the asynchronous state object.
			StateObject state = (StateObject) ar.AsyncState;
			Socket client = state.workSocket;
			
			// Read data from the remote device.
			int bytesRead = client.EndReceive(ar);
			
			m_LastHeartTime = DateTime.Now;
			m_SendedPingCount = 0;
			
			String content = String.Empty;
			if (bytesRead > 0) {
				// There might be more data, so store the data received so far.
				state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));
				
				
				content = state.sb.ToString();

				Debug.Log("content:"+content);
				int eofPos = content.IndexOf("{EOF}");
				if (eofPos > -1)
				{
					content = content.Remove(eofPos);
					state.sb.Remove(0, eofPos+5);
					
					//--m_JobCount;
					
					PacketParser(content);

					// Get the rest of the data.StartLoginJob()
					client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,
					                    new AsyncCallback(ReceiveCallback), state);
				}

			} else {
				
				// connection close;
				
				
				
				// All the data has arrived; put it in response.
				if (state.sb.Length > 1) {
					response = state.sb.ToString();
				}
				
				state.sb.Length = 0;
				
				// Signal that all bytes have been received.
				//receiveDone.Set();
				
				// Write the response to the console.
				Console.WriteLine("Response received : {0}", response);
				Debug.Log("Response received : "+ response);
			}
		} catch (Exception e) {
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
			Debug.Log("received packet type:" + packetType);

			string payload = Utils.Base64Decoding(packet.m_Payload);

			// first get credential
			if (packetType == PacketTypes.PKT_SC_LOGIN && credential != -1 && m_MyCredential == -1)
			{
				m_MyCredential = credential;
			}


			m_PacketHandlerList[(int)packetType].Run(m_MyCredential, payload);
			
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
		
		
	}

	private IEnumerator StartLoginJob()
	{
		yield return null;

		if( null!= m_Client )
		{
			Debug.Log("clear!");
			networkJobList.Clear();
			if(m_Client.Connected)
			{
				m_Client.Shutdown(SocketShutdown.Both);
			}

			m_Client.Close();
			m_Client = null;
        }

		networkJobList.Add( NetworkJob(StartConnect, "Connect Start!") );
		//yield return new WaitForSeconds(0.2f);

		LoginRequest loginRequest = new LoginRequest();
		loginRequest.m_UserKey = m_MyUserKey;
		string packetPayload = JsonFx.Json.JsonWriter.Serialize(loginRequest);
		SendPacket(PacketTypes.PKT_CS_LOGIN, packetPayload);

		//yield return new WaitForSeconds(0.2f);


    }
    
	public void LoginJobDone()
	{
		
		if( !m_IsJoined )
		{
			StartJoinJob();
		}
		
		//yield return new WaitForSeconds(0.2f);
		
		SyncRequest syncRequest = new SyncRequest();
		syncRequest.m_PlayerID = -1; //dummy now
		string syncPayload = JsonFx.Json.JsonWriter.Serialize(syncRequest);
		SendPacket(PacketTypes.PKT_CS_SYNC, syncPayload);

	}

	private void StartJoinJob()
	{

		m_IsJoined = true;

		JoinRequest joinRequest = new JoinRequest();
		joinRequest.m_PlayerID = PlayerManager.GetInstance().m_MyPlayerID;

		string packetPayload = JsonFx.Json.JsonWriter.Serialize(joinRequest);

        
		SendPacket(PacketTypes.PKT_CS_JOIN , packetPayload);


	}

	private void StartPingJob()
	{
		HeartBeatRequest heartBeatRequest = new HeartBeatRequest();

		string packetPayload = JsonFx.Json.JsonWriter.Serialize(heartBeatRequest);
        
		SendPacket(PacketTypes.PKT_CS_PING, packetPayload);
    }
    
    void OnApplicationPause(bool pauseStatus)
	{
		// resume
		if(!pauseStatus)
		{
			StartCoroutine( StartLoginJob() );
		}
	}
	
}
