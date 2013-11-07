using UnityEngine;
using System.Collections;

public class AudioFadeInScript : MonoBehaviour {
	
	private AudioSource source;
	
	private float curVol = 0;
	private float fadeSpeed = 2f; //how quickly to fade in

	
	// Update is called once per frame
	void Update () {
		curVol += fadeSpeed * Time.deltaTime;
		if (curVol > 1) curVol = 1;
		source.volume = curVol;
		//Debug.Log("slide it up "+curVol);
		if (curVol == 1){
			Destroy(this);
		}
	}
	
	public AudioSource Source {
		get {
			return this.source;
		}
		set {
			source = value;
		}
	}
}
