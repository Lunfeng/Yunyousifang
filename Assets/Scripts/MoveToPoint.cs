using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MoveToPoint : MonoBehaviour {

	public GameObject mainScene;
	public GameObject TargetCenter;
	Transform mainTsf;
	Vector3 endScale = Vector3.one;
	public float velocity = 2f;
	static bool isControlAble = true;

	private Touch oldTouch1;  //上次触摸点1(手指1)  
	private Touch oldTouch2;  //上次触摸点2(手指2)  

	// Use this for initialization
	void Start () {
		mainTsf = mainScene.transform;
		TargetCenter = GameObject.Find("TargetCenter");
	}
	
	// Update is called once per frame
	void Update () {
		//没有触摸  

		if (Input.touchCount <= 0)
			{
				return;
			}
		return;
			//多点触摸, 放大缩小  
			Touch newTouch1 = Input.GetTouch(0);
			Touch newTouch2 = Input.GetTouch(1);

			//第2点刚开始接触屏幕, 只记录，不做处理  
			if (newTouch2.phase == TouchPhase.Began)
			{
				oldTouch2 = newTouch2;
				oldTouch1 = newTouch1;
				return;
		}

			//计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
			float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
			float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

			//两个距离之差，为正表示放大手势， 为负表示缩小手势  
			float offset = newDistance - oldDistance;

			//放大因子， 一个像素按 0.01倍来算(100可调整)  
			float scaleFactor = offset / 2000f;
			Vector3 localScale = mainTsf.localScale;
			Vector3 scale = new Vector3(localScale.x + scaleFactor,
				localScale.y + scaleFactor,
				localScale.z + scaleFactor);

			//最小缩放到 0.3 倍  
			if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
			{
				mainTsf.localScale = scale;
			}

			//记住最新的触摸点，下次使用  
			oldTouch1 = newTouch1;
		oldTouch2 = newTouch2;
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
			c.a = Mathf.Log(distence / endScale.x, (float)Math.E);
			Debug.Log(c.a);
			GetComponent<MeshRenderer>().material.color = c;
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
		endScale = Vector3.one * 10;
		if(gameObject.name == "FullViewPoint")
		{
			endScale = Vector3.one;
		}
		Debug.Log("过来！");
		Debug.Log((Vector3.one * 10).magnitude);
		while (!isInPosition(TargetCenter.transform.position, transform.position))
		{
			time += Time.deltaTime;
			startPos = mainTsf.position;
			startScale = mainTsf.localScale;
			endPos = (mainTsf.position + TargetCenter.transform.position - transform.position);

			startPos.x = Mathf.Lerp(startPos.x, endPos.x, (velocity + time * 2) / 50f);
			startPos.y = Mathf.Lerp(startPos.y, endPos.y, (velocity + time * 2) / 50f);
			startPos.z = Mathf.Lerp(startPos.z, endPos.z, (velocity + time * 2) / 50f);

			startScale.x = Mathf.Lerp(startScale.x, endScale.x, (velocity + time * 2) / 100f);
			startScale.y = Mathf.Lerp(startScale.y, endScale.y, (velocity + time * 2) / 100f);
			startScale.z = Mathf.Lerp(startScale.z, endScale.z, (velocity + time * 2) / 100f);

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
