using UnityEngine;
using System.Collections;

public class RoatateCamera : MonoBehaviour {
	//How Senstive the camera movement should be?
	public float senstivity = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//Rotate camera according to input
		transform.eulerAngles += new Vector3 (-Input.GetAxis("Mouse Y") * senstivity,Input.GetAxis("Mouse X") * senstivity,0.0f);

	}
}
