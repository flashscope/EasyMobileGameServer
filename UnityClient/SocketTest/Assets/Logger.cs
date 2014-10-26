using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	private static Logger _instance = null;
	public static Logger GetInstance()
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

	public void Log(string message)
	{
		Debug.Log(message);
	}
}
