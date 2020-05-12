using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChange : MonoBehaviour {

	Toggle mode1;
	Toggle mode2;

	// Use this for initialization
	void Start () {
		mode1 = GameObject.Find("Mode1Toggle").GetComponent<Toggle>();
		mode2 = GameObject.Find("Mode2Toggle").GetComponent<Toggle>();
		mode1.isOn = true;
		mode2.isOn = false;
	}

	public void Mode1On()
	{
		mode1.isOn = true;
		mode2.isOn = false;
	}

	public void Mode2On()
	{
		mode2.isOn = true;
		mode1.isOn = false;
	}

	public void Exit()
	{
		Application.Quit();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
