using UnityEngine;
using System.Collections;


public class FollowCamera : MonoBehaviour {
	//public GameObject target;
	private Transform _target;
	public float damping = 1;
	Vector3 offset;
	private ThirdPersonController controller;
	private Vector3 headOffset = Vector3.zero;
	private Vector3 centerOffset = Vector3.zero;

	void Start() {
		//offset = target.transform.position - transform.position;
		offset = _target.position - transform.position;
	}

	void OnEnable(){
		_target = transform;
		if (_target)
		{
			controller = _target.GetComponent<ThirdPersonController>();
		}

		if (controller)
		{
			CharacterController characterController = (CharacterController)_target.collider;
			centerOffset = characterController.bounds.center - _target.position;
			headOffset = centerOffset;
			headOffset.y = characterController.bounds.max.y - _target.position.y;
		}
		else
			Debug.Log("Please assign a target to the camera that has a ThirdPersonController script attached.");

	}
	
	void LateUpdate() {
		float currentAngle = transform.eulerAngles.y;
		//float desiredAngle = target.transform.eulerAngles.y;
		float desiredAngle = _target.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
		
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		//transform.position = target.transform.position - (rotation * offset);
		transform.position = _target.position - (rotation * offset);

		//transform.LookAt(target.transform);
		transform.LookAt(_target);
	}
}
