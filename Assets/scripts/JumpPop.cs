using UnityEngine;
using System.Collections;

public class JumpPop : MonoBehaviour {
	
	public float time;
	private float timer;
	
	public float startScale, endScale;
	
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		timer = time;
		transform.localScale = new Vector3(startScale,startScale,1);
		
		transform.position += offset;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		
		if (timer <= 0){
			Destroy(gameObject);
		}else{
			float newScale = Mathf.Lerp(endScale, startScale , timer/time);
			transform.localScale = new Vector3(newScale, newScale, 1);
		}
	}
}
