using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour {

	public float moveSpeed = 0.91f;
	public GameObject mainScene;
	Transform mainTsf;
	public float velocity = 1f;
	static bool isControlAble = true;

	// Use this for initialization
	void Start () {
		mainTsf = mainScene.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerStay(Collider other)
	{
		Debug.Log(1);
		if (other.gameObject.tag == "MainCamera")
		{
			float distence = (transform.position - other.transform.position).magnitude;
			Color c = GetComponent<MeshRenderer>().material.color;
			if(distence > 3)
			{
				c.a = distence / 5.56f;
			}
			else
			{
				c.a = Mathf.Pow(distence / 5.56f, 2);
			}
			Debug.Log(c.a);
			GetComponent<MeshRenderer>().material.color = c;
		}
	}

	void OnMouseDown()
	{
		if (isControlAble)
		{
			StartCoroutine(TranslatePosition());
		}
	}

	IEnumerator TranslatePosition()
	{
		isControlAble = false;
		float time = 0;
		Vector3 startPos;
		Vector3 endPos;
		Debug.Log("过来！");
		endPos = (mainTsf.position + Camera.main.transform.position - transform.position);
		while (!isInPosition(Camera.main.transform.position, transform.position))
		{
			time += Time.deltaTime;
			startPos = mainTsf.position;
			startPos.x = Mathf.Lerp(startPos.x, endPos.x, velocity / 100f);
			startPos.y = Mathf.Lerp(startPos.y, endPos.y, velocity / 100f);
			startPos.z = Mathf.Lerp(startPos.z, endPos.z, velocity / 100f);
			mainTsf.position = startPos;
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
