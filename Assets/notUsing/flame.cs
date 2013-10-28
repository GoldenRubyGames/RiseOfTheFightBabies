using UnityEngine;
using System.Collections;

public class flame : MonoBehaviour {
	
	public GameObject triggerObject;
	
	Player owner;
	
	public Vector3 playerBounce;
	
	public float time;
	public float shrinkTime;
	float startScale;
	float timer;
	
	public float shakeRange;

	// Use this for initialization
	void Start () {
		timer = time;
		startScale = transform.localScale.x;
	}
	
	public void setup(Player _owner){
		owner = _owner;
	}
	
	// Update is called once per frame
	void Update () {
		float freshX = Random.Range(-shakeRange, shakeRange);
		float freshY = Random.Range(-shakeRange, shakeRange);
		
		transform.position += new Vector3(freshX, freshY, 0) * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
		timer -= Time.deltaTime;
		
		if (timer < shrinkTime){
			float newScale = (timer/shrinkTime) * startScale;
			transform.localScale = new Vector3(newScale, newScale, newScale);
		}
		
		if (timer <= 0){
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.push(playerBounce);
				thisPlayer.takeDamage(owner, false);
				
			}
		}
	}
}
