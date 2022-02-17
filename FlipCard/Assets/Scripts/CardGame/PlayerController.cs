using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public float Vertical = 0;
	[HideInInspector]
	public float Horizontal = 0;
	public float TurnSpeed = 3;
	public float Speed = 20.0f;


	// Phil For CardGame setting
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;

	public bool isAI;



	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		UpdateInput();


	}

	public void UpdateInput()
	{
		Horizontal = Input.GetAxis("Horizontal");
		Vertical = Input.GetAxis("Vertical");
	}

	public void Rotating(float hor, float ver)
	{
		Vector3 dir = new Vector3(hor, 0, ver);
		Quaternion quaDir = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * TurnSpeed);
	}


	public void Translate(float hor, float ver)
	{

		Vector3 dir = new Vector3(hor, 0, ver);

		//GetComponent<Rigidbody>().velocity = dir * 10;
		Vector3 transformValue = dir * Time.fixedDeltaTime * Speed;
		transform.Translate(transformValue, Space.World);
		// boundary limitation
		Vector3 lastPosition = new Vector3(Mathf.Clamp(this.transform.position.x, minX, maxX), 0,
			Mathf.Clamp(this.transform.position.z, minZ, maxZ));
		this.transform.position = lastPosition;

	}

	void FixedUpdate()
	{
		StartCoroutine(InternalUpdate());

	}

	IEnumerator InternalUpdate()
	{
		if (!isAI)
		{
			if (Horizontal != 0 || Vertical != 0)
			{
				Rotating(Horizontal, Vertical);
				Translate(Horizontal, Vertical);
			}
		}
		yield return null;
	}

}
