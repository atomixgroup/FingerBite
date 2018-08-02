using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
	public delegate void OnFinishCallback();
	public OnFinishCallback onFinishCallback;
	
	public GameObject loadingPabel;
	private float fadeSpeed=1f;
	private bool fadeOutStart=false;
	public GameObject fadePanel;
	private Image panelBlack;
	private float opacity = 0f;
	void Start ()
	{
		panelBlack = fadePanel.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeOutStart)
		{
			opacity += Time.deltaTime * fadeSpeed;
			panelBlack.color=new Color(0f,0f,0f,opacity);
			if (opacity >= 1f)
			{
				fadeOutStart = false;
				if (loadingPabel != null) 
				{
					loadingPabel.SetActive(true);
				}
				if (onFinishCallback != null)
					onFinishCallback.Invoke ();
			}
		}
	}

	public void startFadeOut()
	{
		fadePanel.SetActive(true);
		fadeOutStart = true;
	}
}
