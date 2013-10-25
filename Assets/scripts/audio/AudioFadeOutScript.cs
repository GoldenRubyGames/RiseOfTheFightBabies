using UnityEngine;
using System.Collections;

public class AudioFadeOutScript : MonoBehaviour {
	
	private AudioSource source;
	
	private float curVol = 0;
	private float fadeSpeed = 2f; //how quickly to fade in
	
	void Start(){
		curVol = source.volume;
	}
	
	// Update is called once per frame
	void Update () {
		curVol -= fadeSpeed * Time.deltaTime;
		if (curVol <0) curVol = 0;
		source.volume = curVol;
		Debug.Log("slide it down "+curVol);
		if (curVol == 0){
			Destroy(gameObject);
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
