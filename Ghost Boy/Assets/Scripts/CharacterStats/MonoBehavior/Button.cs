using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{

	[SerializeField]
	Image jumpForceBar;
	CharacterMove CM;
	float holdTime;

	// Use this for initialization
	void Start()
	{
		jumpForceBar.fillAmount = 0f;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			jumpForceBar.fillAmount = 0f;
			StartCoroutine("StartCounting");
		}	
	}

	public void OnPointerUp()
	{
		StopCoroutine("StartCounting");

		if (holdTime < 0.3f)
		{

		}
			//CM.DoJump(300f);
		else
			//CM.DoJump(holdTime * 1000f);

		jumpForceBar.fillAmount = 0f;
	}

	IEnumerator StartCounting()
	{
		for (holdTime = 0f; holdTime <= 1f; holdTime += Time.deltaTime)
		{
			jumpForceBar.fillAmount = holdTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		holdTime = 1f;
		jumpForceBar.fillAmount = holdTime;
	}

}
