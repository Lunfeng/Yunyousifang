using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MoveToPoint : MonoBehaviour {

	public GameObject mainScene;
	public GameObject TargetCenter;
	Transform mainTsf;
	public float velocity = 1f;
	static bool isControlAble = true;

	// Use this for initialization
	void Start () {
		mainTsf = mainScene.transform;
		TargetCenter = GameObject.Find("TargetCenter");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		if (isControlAble)
		{
			StartCoroutine(TranslatePosition());
			StartCoroutine(FadeImage());
		}
	}

	IEnumerator FadeImage()
	{
		Debug.Log("开始了！");

		Color c = GetComponent<MeshRenderer>().material.color;
		while (true)
		{
			float distence = (transform.position - TargetCenter.transform.position).magnitude;
			Debug.Log(distence);
			if(distence < 3)
			{
				c.a = distence * 3 - 1;
				Debug.Log(c.a);
				GetComponent<MeshRenderer>().material.color = c;
			}
			if (isInPosition(TargetCenter.transform.position, transform.position))
			{
				yield break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator TranslatePosition()
	{
		isControlAble = false;
		float time = 0;
		Vector3 startPos;
		Vector3 startScale;
		Vector3 endPos;
		Vector3 endScale = Vector3.one * 10;
		if(gameObject.name == "FullViewPoint")
		{
			endScale = Vector3.one;
		}
		Debug.Log("过来！");
		while (!isInPosition(TargetCenter.transform.position, transform.position))
		{
			time += Time.deltaTime;
			startPos = mainTsf.position;
			startScale = mainTsf.localScale;
			endPos = (mainTsf.position + TargetCenter.transform.position - transform.position);

			startPos.x = Mathf.Lerp(startPos.x, endPos.x, velocity / 100f);
			startPos.y = Mathf.Lerp(startPos.y, endPos.y, velocity / 100f);
			startPos.z = Mathf.Lerp(startPos.z, endPos.z, velocity / 100f);

			startScale.x = Mathf.Lerp(startScale.x, endScale.x, velocity / 200f);
			startScale.y = Mathf.Lerp(startScale.y, endScale.y, velocity / 200f);
			startScale.z = Mathf.Lerp(startScale.z, endScale.z, velocity / 200f);

			mainTsf.position = startPos;
			mainTsf.localScale = startScale;
			yield return new WaitForFixedUpdate();
		}
		Debug.Log("到了！");
		isControlAble = true;
		yield break;
	}

	private bool isInPosition(Vector3 position1, Vector3 position2)
	{
		bool x = false;
		if (Mathf.Abs(position2.x - position1.x) <= 0.2f)
			x = true;
		bool y = false;
		if (Mathf.Abs(position2.y - position1.y) <= 0.2f)
			y = true;
		bool z = false;
		if (Mathf.Abs(position2.z - position1.z) <= 0.2f)
			z = true;
		if (x & y & z)
			return true;
		return false;
	}
}
