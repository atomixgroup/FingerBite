using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordRowSetter : MonoBehaviour
{
	public Text name, record;
	// Use this for initialization
	private void Awake()
	{
		name = transform.GetChild(0).GetComponent<Text>();
		record = transform.GetChild(1).GetComponent<Text>();
	}

	public void SetRecord(string name, string record)
	{
		this.record.text = record;
		this.name.text = name;
	}
	
}
