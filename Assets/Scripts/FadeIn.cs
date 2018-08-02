using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
	public float fadeSpeed = 1f;
	private Image blackPanel;

	private float opacity = 1f;
	// Use this for initialization
	void Start ()
	{
		blackPanel = GetComponent<Image>();
		opacity = 1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		opacity -= Time.deltaTime * fadeSpeed;
		blackPanel.color=new Color(0f,0f,0f,opacity);
		if (opacity <= 0)
		{
			Destroy(gameObject);
		}
	}
}
