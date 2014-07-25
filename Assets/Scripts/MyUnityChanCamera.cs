using UnityEngine;
using System.Collections;

public class MyUnityChanCamera : MonoBehaviour {
	
	public float cameraDistance;
	
	private int angle = 0;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if(player)
		{
			if(Input.GetKey( KeyCode.Q ))
			{
				angle -= 5;
				angle %= 360;
				transform.rotation = Quaternion.AngleAxis( angle,new Vector3(0,1,0));
			}
			else if(Input.GetKey( KeyCode.W ))
			{
				angle += 5;
				angle %= 360;
				transform.rotation = Quaternion.AngleAxis( angle,new Vector3(0,1,0));
			}
			transform.position = player.transform.position;
			transform.position += Vector3.up * 1.0f;
			transform.position -= transform.forward * cameraDistance;
			
		}
	}
}
