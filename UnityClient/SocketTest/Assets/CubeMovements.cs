using UnityEngine;
using System.Collections;

public class CubeMovements : MonoBehaviour {

	void Update ()
	{
		Vector3 r = new Vector3 (1.0f, 1.0f, 0.0f);
		transform.Rotate (r);
	}
}
