using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDust : MonoBehaviour {

	public PlayerController PC;
	[SerializeField]
	GameObject dustCloud;
	public Transform dustPos;
	public bool isCreated;

	private void Update()
	{
		if (PC.isGrounded && !isCreated)
		{
			Instantiate(dustCloud, dustPos.transform.position, dustCloud.transform.rotation);
			isCreated = true;
		}
		if (!PC.isGrounded)
		{
			isCreated = false;
		}
	}
}
