using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	public Transform camCam;
	public Vector3 posCam;
	public Quaternion lastPos;
	public float turnSpeed = 20f;

	private void Awake()
	{
		camCam = Camera.main.transform;
	}

	private void Update()
	{
		posCam = camCam.position - transform.position;
		posCam.y = 0;
		lastPos = Quaternion.LookRotation(-posCam);
		transform.rotation = Quaternion.Slerp(transform.rotation, lastPos, Time.deltaTime * turnSpeed);
	}
}
