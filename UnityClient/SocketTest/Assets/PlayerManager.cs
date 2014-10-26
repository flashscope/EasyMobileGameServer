using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	[SerializeField]
	private GameObject m_PlayerObject = null;

	private List<JoinResult> joinList = new List<JoinResult>();
	private List<MoveResult> moveList = new List<MoveResult>();

	public int m_MyPlayerID = -1;
	private Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>(); // playerID


	private static PlayerManager _instance = null;
	public static PlayerManager GetInstance()
	{
		return _instance;
	}
	
	void Start ()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}



	void Update ()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			if( !m_PlayerList.ContainsKey(m_MyPlayerID) )
			{
				// not receive join
				return;
			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit))
			{
				// send to server my move
				GameObject myPlayer = m_PlayerList[m_MyPlayerID];
				PlayerController myPlayerControl = myPlayer.GetComponent<PlayerController>();
				Vector3 rayPos = hit.point;
				
				Vector3 vector = transform.position - rayPos;
				Vector3 normal = vector.normalized;
				float angle = NormalAngle(normal);
				angle -= 90;
				try
				{
					MoveRequest moveRequestPay = new MoveRequest();
					moveRequestPay.m_PlayerID = myPlayerControl.m_PlayerID;
					moveRequestPay.m_PosX = rayPos.x;
					moveRequestPay.m_PosY = rayPos.y;
					moveRequestPay.m_PosZ = rayPos.z;
					moveRequestPay.m_Angle = angle;
					moveRequestPay.m_Speed = myPlayerControl.m_Speed;
					
					string requestPayload = JsonFx.Json.JsonWriter.Serialize(moveRequestPay);

					NetworkManager.GetInstance().SendPacket(PacketTypes.PKT_CS_MOVE, requestPayload);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
				
				
			}
			
			
		}

		if(joinList.Count > 0)
		{

			JoinResult packet = joinList[0];
			//Debug.Log("X:"+packet.m_PosX+" Y:"+packet.m_PosY+" Z:"+ packet.m_PosZ);

			if(packet.m_PlayerID == -4444)
			{
				foreach (GameObject player in m_PlayerList.Values)
				{
					Destroy(player);
				}
				
				m_PlayerList.Clear();
				joinList.RemoveAt(0);
				return;
			}


			Vector3 pos = new Vector3(packet.m_PosX, packet.m_PosY, packet.m_PosZ);
			Vector3 rot = new Vector3(0.0f, packet.m_Angle, 0.0f);
			float speed = packet.m_Speed;
			int playerID = packet.m_PlayerID;
			
			GameObject newPlayer = Instantiate(m_PlayerObject, pos, Quaternion.Euler(rot)) as GameObject;
			PlayerController newPlayerControl = newPlayer.GetComponent<PlayerController>();
			
			newPlayerControl.m_Speed = speed;
			newPlayerControl.m_PlayerID = playerID;
			
			m_PlayerList.Add(playerID, newPlayer);

			joinList.RemoveAt(0);
		}

		if(moveList.Count > 0)
		{
			MoveResult packet = moveList[0]; 

			try
			{
				GameObject player = m_PlayerList[packet.m_PlayerID];
				PlayerController playerControl = player.GetComponent<PlayerController>();
				playerControl.AddMoveResult(packet);
			}
			catch( Exception e )
			{
				//no character sync
				Debug.LogError(e);
			}
			moveList.RemoveAt(0);
		}
	}

	public void AddNewPlayer(JoinResult packet)
	{
		joinList.Add(packet);
		Debug.Log("AddNewPlayer!:" + packet.m_PlayerID);
	}


	public void AddMoveResult(MoveResult packet)
	{
		moveList.Add(packet);
		Debug.Log("AddMoveResult!:" + packet.m_PlayerID);
	}

	public void ManagerClear()
	{
		joinList.Clear();
		moveList.Clear();

		JoinResult destroyer = new JoinResult();
		destroyer.m_PlayerID = -4444;
	
		joinList.Add(destroyer);

	}

	public float NormalAngle(Vector3 normal)
	{
		float rad = Mathf.Atan2(normal.z, normal.x);
		float angle = -rad * Mathf.Rad2Deg;
		
		if( angle < 0.0f )
		{
			angle += 360;
		}
		return angle;
	}
}
