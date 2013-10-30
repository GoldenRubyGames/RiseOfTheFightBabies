using UnityEngine;
using System.Collections;

public class RoundFlash : MonoBehaviour {
	
	public tk2dSprite sprite;
	
	public float fadeTime;
	private float timer;
	
	private float curAlpha;
	
	
	// Update is called once per frame
	void Update () {
		
		timer -= Time.deltaTime;
		float curAlpha = Mathf.Max(0, timer/fadeTime);
		
		sprite.color = new Color( sprite.color.r, sprite.color.g, sprite.color.b, curAlpha);
		
		if (timer <= 0){
			gameObject.SetActive(false);
		}
	}
	
	public void activate(){
		timer = fadeTime;
		gameObject.SetActive(true);
	}
}
