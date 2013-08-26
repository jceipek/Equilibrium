using UnityEngine;
using System.Collections;

public class QuitWithEsc : MonoBehaviour {
	void Update ()
	{
		if (Input.GetKey("escape"))
		{
			Debug.Log("QUIT!");
			Application.Quit();
		}
	}
}
