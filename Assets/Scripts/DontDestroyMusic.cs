using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour {
	private void Awake()
	{
		GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
		if (musicObjects.Length > 1)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
}
