using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour {

	public GameObject SettingButton;
	public GameObject SettingPanel;
	public GameObject Mode1Guide;
	public GameObject Mode2Guide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void exit()
	{
		Application.Quit();
	}
}
