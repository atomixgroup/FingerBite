using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public GameObject lightCanvas;
	private bool awailble = true;

	private AudioSource[] lightningAudio;
	// Use this for initialization
	void Start ()
	{
		lightningAudio = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (awailble)
		{
			awailble = false;
			StartCoroutine(MakeLightning(Random.Range(10, 15)));
		}
	}

	public IEnumerator MakeLightning(float wait)
	{
		while (true)
		{
			yield return new WaitForSeconds(wait);
			int myRandomIndex=0;
			myRandomIndex = Random.Range( 0, 2 );
			print(myRandomIndex);
			lightningAudio[myRandomIndex].Play();
			lightCanvas.SetActive(true);
			yield return new WaitForSeconds(Random.Range(.02f,.1f));
			lightCanvas.SetActive(false);
			yield return new WaitForSeconds(Random.Range(.02f,.1f));
			lightCanvas.SetActive(true);
			yield return new WaitForSeconds(Random.Range(.02f,.1f));
			lightCanvas.SetActive(false);
			yield return new WaitForSeconds(Random.Range(.02f,.1f));
			lightCanvas.SetActive(true);
			yield return new WaitForSeconds(Random.Range(.02f,.1f));
			lightCanvas.SetActive(false);
			break;
			
		}
		awailble = true;
	} 
	
}
