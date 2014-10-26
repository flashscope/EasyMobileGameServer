using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour {


	public float m_Speed = 1.0f;
	public int m_PlayerID = -1;

	private List<MoveResult> moveList = new List<MoveResult>();
    
	void Start()
	{
		ChangeAnimation(QueryAnimationController.QueryChanAnimationType.STAND);
	}

	void Update ()
	{
		if(moveList.Count > 0 )
		{
			MoveResult moveResult = moveList[0];


			Vector3 tarPos = new Vector3(moveResult.m_PosX, moveResult.m_PosY, moveResult.m_PosZ);
			Vector3 vector = transform.position - tarPos;
			Vector3 normal = vector.normalized;
			
			float angle = NormalAngle(normal);
			angle -= 90;
			gameObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));
			
			
			Hashtable hash = new Hashtable();
			hash.Add("position", tarPos);
            hash.Add("speed", moveResult.m_Speed);
			hash.Add("oncomplete", "MoveComplete");

            iTween.MoveTo(gameObject, hash);
			ChangeAnimation(QueryAnimationController.QueryChanAnimationType.RUN);

			moveList.RemoveAt(0);
		}


	}

	void MoveComplete()
	{
		ChangeAnimation(QueryAnimationController.QueryChanAnimationType.STAND);
    }

	public void AddMoveResult(MoveResult moveResult)
	{
		moveList.Add(moveResult);
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

	void ChangeAnimation (QueryAnimationController.QueryChanAnimationType animNumber)
	{
		gameObject.GetComponent<QueryAnimationController>().ChangeAnimation(animNumber);
    }

}
