using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class Webservice  {
	public delegate void OnFinishCallback(string message);
	public  OnFinishCallback OnRecieve;
	public  OnFinishCallback OnError;
	public bool inProgress = false;

	public  IEnumerator GetRequest(string uri)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(uri);
		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError)
		{
			
			OnError.Invoke (uwr.error);
		}
		else
		{
			
				OnRecieve.Invoke (uwr.downloadHandler.text);
		}
	}
	public IEnumerator PostRequest(string url,WWWForm form)
	{
		
		UnityWebRequest uwr = UnityWebRequest.Post(url, form);
		yield return uwr.SendWebRequest();
		
		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
			if (OnError != null)
				OnError.Invoke (uwr.error);
		}
		else
		{
			Debug.Log("Received: " + uwr.downloadHandler.text);
			if (OnRecieve != null)
				OnRecieve.Invoke (uwr.downloadHandler.text);
		}
		inProgress = false;
	}

	
}
