using UnityEngine;
using System.Collections;

public class ClickToMove : Photon.MonoBehaviour
{
	//private Transform target;
	private Vector3 target;
	private Vector3	direction = Vector3.zero; 

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 vec = Input.mousePosition;
			vec.z = 10f;
			
			//transform.position = camera.ScreenToWorldPoint(vec);
			target = camera.ScreenToWorldPoint(vec);

			direction = (camera.ScreenToWorldPoint(vec) - transform.position).normalized; 

		}
		//transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 10/*speed*/);
		transform.position += direction * Time.deltaTime;
	}
}
