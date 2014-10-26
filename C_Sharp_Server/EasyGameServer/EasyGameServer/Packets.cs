using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer
{
    public enum PacketTypes
    {
        PKT_NONE,
        PKT_CS_LOGIN,
        PKT_SC_LOGIN,
        PKT_CS_JOIN,
        PKT_SC_JOIN,
        PKT_CS_OUT,
        PKT_SC_OUT,
        PKT_CS_MOVE,
        PKT_SC_MOVE,
        PKT_CS_SYNC,
        PKT_SC_SYNC,
        PKT_CS_PING,
        PKT_SC_PONG,
        PKT_CS_ERROR,
        PKT_SC_ERROR,
        PKT_MAX
    }

    public class Packet
    {
        public Packet()
        {
            m_Credential = -1;
            m_Type = -1;
            m_Payload = string.Empty;
        }

        public int m_Credential;
        public int m_Type;
        public string m_Payload;
    }

    public class LoginRequest
    {
        public LoginRequest()
        {
            m_UserKey = string.Empty;
        }
        
        public string m_UserKey;
    }

    public class LoginResult
    {
        public LoginResult()
        {
            m_PlayerID = -1;
        }

        public int m_PlayerID;
    }

    public class JoinRequest
    {
        public JoinRequest()
        {
            m_PlayerID = -1;
        }

        public int m_PlayerID;
    }

    public class JoinResult
    {
        public JoinResult()
        {
            m_PlayerID = -1;
            m_PosX = float.MinValue;
            m_PosY = float.MinValue;
            m_PosZ = float.MinValue;
            m_Angle = float.MinValue;
            m_Speed = float.MinValue;
        }

        public int m_PlayerID;
        public float m_PosX;
        public float m_PosY;
        public float m_PosZ;
        public float m_Angle;
        public float m_Speed;
    }

    public class SyncRequest
    {
        public SyncRequest()
        {
            m_PlayerID = -1;
        }

        public int m_PlayerID;
    }

    public class SyncResult
    {
        public SyncResult()
        {
            m_JoinList = null;
        }

        public List<JoinResult> m_JoinList;
    }

    public class MoveRequest
    {
        public MoveRequest()
        {
            m_PlayerID = -1;
            m_PosX = float.MinValue;
            m_PosY = float.MinValue;
            m_PosZ = float.MinValue;
            m_Angle = float.MinValue;
            m_Speed = float.MinValue;
        }

        public int m_PlayerID;
        public float m_PosX;
        public float m_PosY;
        public float m_PosZ;
        public float m_Angle;
        public float m_Speed;
    }

    public class MoveResult
    {
        public MoveResult()
        {
            m_PlayerID = -1;
            m_PosX = float.MinValue;
            m_PosY = float.MinValue;
            m_PosZ = float.MinValue;
            m_Angle = float.MinValue;
            m_Speed = float.MinValue;
        }

        public int m_PlayerID;
        public float m_PosX;
        public float m_PosY;
        public float m_PosZ;
        public float m_Angle;
        public float m_Speed;
    }

    public class HeartBeatRequest
    {
        public HeartBeatRequest()
        {
            m_Dummy = "PING";
        }

        public string m_Dummy;
    }

    public class HeartBeatResult
    {
        public HeartBeatResult()
        {
            m_Dummy = "PONG";
        }

        public string m_Dummy;
    }

}
