using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
	private Image fillImage;
	private Text text;
	private Text maxSpeed;
	// Use this for initialization
	void Start ()
	{
		fillImage = transform.GetChild(0).GetComponent<Image>();
		text = transform.GetChild(3).GetComponent<Text>();
		maxSpeed = transform.GetChild(2).GetComponent<Text>();
		maxSpeed.text = "Max Speed " + (MenuManager.attackingSpeedMax*10);
		setSliderValue();
	}
	
	// Update is called once per frame


	public void setSliderValue()
	{
		float ratio = MenuManager.attackingSpeed / MenuManager.attackingSpeedMax;
		if (ratio > 1)
		{
			ratio = 1;
		}
		fillImage.rectTransform.localScale=new Vector3(1,ratio,1);
		text.text = (MenuManager.attackingSpeed*10)+"";

	}
}
