using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour {

	GameObject ARCamera;
	// Use this for initialization
	void Start () {
		ARCamera = GameObject.Find("ARCamera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(ARCamera.transform);
	}
}
