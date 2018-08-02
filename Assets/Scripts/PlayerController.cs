using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private GameController gm;
	public ParticleSystem blood;
	private Webservice _webservice;
	
	void Start ()
	{
		_webservice=new Webservice();
		gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
	}
	
	void Update ()
	{
		if (transform.position != gm.ScreenToWorldFingerPos)
			transform.position = Vector3.Lerp (transform.position,
				new Vector3(gm.ScreenToWorldFingerPos.x, gm.ScreenToWorldFingerPos.y, transform.position.z),
				Time.deltaTime * 6.0f);
		if (gm.isTouched) gameObject.GetComponent<Collider2D>().enabled = true; 
		else gameObject.GetComponent<Collider2D>().enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!_webservice.inProgress)
		{
			_webservice.inProgress = true;
			SendRecord();

		}
		
		gm.isCollided = true;
		blood.Play();
		

	}

	private IEnumerator Wait(int second)
	{
		while (true)
		{
			yield return new WaitForSeconds(second);
			SendRecord();
			break;
		}
	}

	private void SendRecord()
	{
		_webservice.inProgress = true;
		WWWForm form=new WWWForm();
		form.AddField("name",ObscuredPrefs.GetString("name"));
		form.AddField("type",ObscuredPrefs.GetInt("type"));
		form.AddField("id",ObscuredPrefs.GetString("unique"));
		form.AddField("record",MenuManager.score);
		StartCoroutine(_webservice.PostRequest("http://game.codetower.ir/api/setRecord", form));
	}
}